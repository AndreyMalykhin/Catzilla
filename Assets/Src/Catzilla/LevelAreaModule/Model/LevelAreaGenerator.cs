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
        [Inject]
        public ObjectTypeInfoStorage ObjectTypeInfoStorage {get; set;}

        [Inject("PlayerObjectType")]
        public LevelObjectType PlayerObjectType {get; set;}

        private readonly IDictionary<Vector3, bool> reservedSpawnPoints =
            new Dictionary<Vector3, bool>(32, new Vector3Comparer());

        public void NewArea(
            EnvTypeInfo envTypeInfo,
            List<SpawnsInfo> spawnsInfos,
            LevelSettings levelSettings,
            LevelView outputLevel,
            Action<LevelAreaView> onDone = null) {
            // DebugUtils.Log(
            //     "LevelAreaGenerator.NewArea(); envType={0}", envType);
            outputLevel.StartCoroutine(DoNewArea(
                envTypeInfo, spawnsInfos, levelSettings, outputLevel, onDone));
        }

        private IEnumerator DoNewArea(
            EnvTypeInfo envTypeInfo,
            List<SpawnsInfo> spawnsInfos,
            LevelSettings levelSettings,
            LevelView outputLevel,
            Action<LevelAreaView> onDone = null) {
            reservedSpawnPoints.Clear();
            LevelAreaView area = outputLevel.NewArea(envTypeInfo);

            for (int i = 0; i < spawnsInfos.Count; ++i) {
                SpawnsInfo spawnsInfo = spawnsInfos[i];
                IEnumerator objects = NewObjects(
                    spawnsInfo.ObjectType,
                    spawnsInfo.Count,
                    envTypeInfo,
                    area.Index,
                    levelSettings,
                    outputLevel);

                while (objects.MoveNext()) {
                    yield return null;
                }
            }

            if (onDone != null) {
                onDone(area);
            }
        }

        private IEnumerator NewObjects(
            LevelObjectType objectType,
            int countToSpawn,
            EnvTypeInfo envTypeInfo,
            int areaIndex,
            LevelSettings levelSettings,
            LevelView outputLevel) {
            // DebugUtils.Log("LevelAreaGenerator.NewObjects()");
            List<SpawnLocation> spawnLocations =
                envTypeInfo.SpawnMap.GetLocations(objectType);
            ObjectTypeInfo objectTypeInfo =
                ObjectTypeInfoStorage.Get(objectType);
            Vector3 spawnPoint;
            SpawnLocation spawnLocation;
            int triesCount = countToSpawn * 2;
            bool isFreeSpaceExists = true;

            for (int i = 0; i < countToSpawn; ++i) {
                do {
                    spawnLocation = spawnLocations[
                        UnityEngine.Random.Range(0, spawnLocations.Count)];
                    spawnPoint = GetRandomSpawnPoint(
                        spawnLocation.Bounds, objectTypeInfo);

                    if (triesCount == 0) {
                        isFreeSpaceExists = false;
                        break;
                    }

                    --triesCount;
                } while (IsSpawnPointReserved(spawnPoint));

                if (!isFreeSpaceExists) {
                    break;
                }

                int objectProtoIndex = UnityEngine.Random.Range(
                    0, objectTypeInfo.ProtoInfos.Length);
                ObjectProtoInfo objectProtoInfo =
                    objectTypeInfo.ProtoInfos[objectProtoIndex];
                LevelObjectView obj = outputLevel.NewObject(
                    objectProtoInfo.View, spawnPoint, areaIndex);
                InitObject(obj, objectTypeInfo, objectProtoInfo, spawnLocation,
                    levelSettings);
                ReserveSpawnPoint(spawnPoint);
                yield return null;
            }
        }

        private Vector3 GetRandomSpawnPoint(
            Bounds spawnLocation, ObjectTypeInfo objectTypeInfo) {
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

        private void InitObject(
            LevelObjectView obj,
            ObjectTypeInfo objectTypeInfo,
            ObjectProtoInfo objectProtoInfo,
            SpawnLocation spawnLocation,
            LevelSettings levelSettings) {
            obj.transform.rotation =
                Quaternion.Euler(0f, spawnLocation.IsXFlipped ? 180f : 0f, 0f);
            var alternating = obj.GetComponent<AlternatingView>();

            if (alternating != null) {
                int materialIndex = UnityEngine.Random.Range(
                    0, objectProtoInfo.AvailableMaterials.Length);
                alternating.Material =
                    objectProtoInfo.AvailableMaterials[materialIndex];
            }

            if (objectTypeInfo.Type == PlayerObjectType) {
                var player = obj.GetComponent<PlayerView>();
                player.FrontSpeed = levelSettings.PlayerFrontSpeed;
                player.SideSpeed = levelSettings.PlayerSideSpeed;
            }
        }
    }
}
