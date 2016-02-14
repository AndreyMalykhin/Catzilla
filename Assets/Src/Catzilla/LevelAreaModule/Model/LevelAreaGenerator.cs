using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.LevelObjectModule.Model;
using Catzilla.LevelObjectModule.View;
using Catzilla.LevelModule.Model;
using Catzilla.LevelModule.View;
using Catzilla.LevelAreaModule.View;

namespace Catzilla.LevelAreaModule.Model {
    public class LevelAreaGenerator {
        private struct ObjectSpawnsInfo {
            public LevelObjectType Type;
            public int Count;
        }

        [Inject]
        public ObjectTypeInfoStorage ObjectTypeInfoStorage {get; set;}

        [Inject]
        public EnvTypeInfoStorage EnvTypeInfoStorage {get; set;}

        [Inject]
        public LevelSettingsStorage LevelSettingsStorage {get; set;}

        [Inject("PlayerObjectType")]
        public LevelObjectType PlayerObjectType {get; set;}

        private readonly IDictionary<Vector3, bool> reservedSpawnPoints =
            new Dictionary<Vector3, bool>();

        public void NewArea(
            EnvType envType,
            bool spawnPlayer,
            LevelView outputLevel,
            Action<LevelAreaView> onDone = null) {
            // DebugUtils.Log(
            //     "LevelAreaGenerator.NewArea(); envType={0}", envType);
            outputLevel.StartCoroutine(DoNewArea(
                envType, spawnPlayer, outputLevel, onDone));
        }

        private IEnumerator DoNewArea(
            EnvType envType,
            bool spawnPlayer,
            LevelView outputLevel,
            Action<LevelAreaView> onDone = null) {
            reservedSpawnPoints.Clear();
            EnvTypeInfo envTypeInfo = EnvTypeInfoStorage.Get(envType);
            LevelSettings levelSettings =
                LevelSettingsStorage.Get(outputLevel.Index);
            ObjectSpawnsInfo[] objectTypesToSpawn = GetObjectTypesToSpawn(
                levelSettings, envTypeInfo, spawnPlayer);
            LevelAreaView area = outputLevel.NewArea(envTypeInfo);

            for (int i = 0; i < objectTypesToSpawn.Length; ++i) {
                ObjectSpawnsInfo objectTypeToSpawn = objectTypesToSpawn[i];
                NewObjects(
                    objectTypeToSpawn.Type,
                    objectTypeToSpawn.Count,
                    envTypeInfo,
                    spawnPlayer,
                    area.Index,
                    levelSettings,
                    outputLevel);
                yield return null;
            }

            if (onDone != null) {
                onDone(area);
            }
        }

        private void NewObjects(
            LevelObjectType objectType,
            int countToSpawn,
            EnvTypeInfo envTypeInfo,
            bool spawnPlayer,
            int areaIndex,
            LevelSettings levelSettings,
            LevelView outputLevel) {
            List<Bounds> spawnLocations =
                envTypeInfo.SpawnMap.GetLocations(objectType);
            ObjectTypeInfo objectTypeInfo =
                ObjectTypeInfoStorage.Get(objectType);
            Vector3 spawnPoint;
            int triesCount = countToSpawn * 2;

            for (int i = 0; i < countToSpawn; ++i) {
                do {
                    spawnPoint =
                        GetRandomSpawnPoint(spawnLocations, objectTypeInfo);
                    --triesCount;
                } while (IsSpawnPointReserved(spawnPoint) && triesCount > 0);

                if (triesCount == 0) {
                    break;
                }

                LevelObjectView obj = outputLevel.NewObject(
                    objectTypeInfo, spawnPoint, areaIndex);
                InitObject(obj, objectTypeInfo.Type, levelSettings);
                ReserveSpawnPoint(spawnPoint);
            }
        }

        private Vector3 GetRandomSpawnPoint(
            List<Bounds> spawnLocations, ObjectTypeInfo objectTypeInfo) {
            var spawnLocation = spawnLocations[
                UnityEngine.Random.Range(0, spawnLocations.Count)];
            Vector3 spawnLocationSize = spawnLocation.size;
            int objectWidth = objectTypeInfo.Width;
            int objectDepth = objectTypeInfo.Depth;
            DebugUtils.Assert(spawnLocationSize.x >= objectWidth);
            DebugUtils.Assert(spawnLocationSize.z >= objectDepth);
            int x = UnityEngine.Random.Range(
                0,
                Mathf.RoundToInt(spawnLocationSize.x) / objectWidth);
            int z = UnityEngine.Random.Range(
                0,
                Mathf.RoundToInt(spawnLocationSize.z) / objectDepth);
            Vector3 spawnLocationStart = spawnLocation.min;
            return new Vector3(
                spawnLocationStart.x + objectWidth / 2f + x * objectWidth,
                spawnLocation.center.y,
                spawnLocationStart.z + objectDepth / 2f + z * objectDepth);
        }

        private bool IsSpawnPointReserved(Vector3 point) {
            return reservedSpawnPoints.ContainsKey(point);
        }

        private void ReserveSpawnPoint(Vector3 point) {
            reservedSpawnPoints[point] = true;
        }

        private ObjectSpawnsInfo[] GetObjectTypesToSpawn(
            LevelSettings levelSettings,
            EnvTypeInfo envTypeInfo,
            bool spawnPlayer) {
            LevelObjectType[] objectTypes =
                envTypeInfo.SpawnMap.GetObjectTypes();
            Array.Sort(objectTypes, ObjectTypeComparator);
            int objectTypesCount = objectTypes.Length;
            var objectSpawnsInfos = new ObjectSpawnsInfo[objectTypesCount];

            for (int i = 0; i < objectTypesCount; ++i) {
                objectSpawnsInfos[i] =
                    new ObjectSpawnsInfo{Type = objectTypes[i]};
            }

            int dangerousObjectsLeft = levelSettings.AreaDangerousObjects;
            int scoreableObjectsLeft = levelSettings.AreaScoreableObjects;
            int objectsCount = dangerousObjectsLeft + scoreableObjectsLeft;

            for (int i = 0; i < objectsCount; ++i) {
                int j = i % objectTypesCount;
                ObjectSpawnsInfo objectSpawnsInfo = objectSpawnsInfos[j];
                LevelObjectType objectType = objectSpawnsInfo.Type;
                LevelObjectView objectProto =
                    ObjectTypeInfoStorage.Get(objectType).ViewProto;

                if (spawnPlayer && objectType == PlayerObjectType) {
                    spawnPlayer = false;
                } else if (objectProto.GetComponent<DangerousView>() != null) {
                    if (dangerousObjectsLeft <= 0) {
                        continue;
                    }

                    --dangerousObjectsLeft;
                } else if (objectProto.GetComponent<ScoreableView>() != null) {
                    if (scoreableObjectsLeft <= 0) {
                        continue;
                    }

                    --scoreableObjectsLeft;
                } else {
                    continue;
                }

                ++objectSpawnsInfo.Count;
                objectSpawnsInfos[j] = objectSpawnsInfo;
            }

            return objectSpawnsInfos;
        }

        private int ObjectTypeComparator(
            LevelObjectType lhs, LevelObjectType rhs) {
            return ObjectTypeInfoStorage.Get(lhs).SpawnPriority.CompareTo(
                ObjectTypeInfoStorage.Get(rhs).SpawnPriority);
        }

        private void InitObject(
            LevelObjectView obj,
            LevelObjectType objectType,
            LevelSettings levelSettings) {
            if (objectType == PlayerObjectType) {
                var player = obj.GetComponent<PlayerView>();
                player.FrontSpeed = levelSettings.PlayerFrontSpeed;
                player.SideSpeed = levelSettings.PlayerSideSpeed;
            }
        }
    }
}
