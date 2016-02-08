﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Catzilla.CommonModule.Util;
using Catzilla.LevelObjectModule.Model;
using Catzilla.LevelModule.View;
using Catzilla.LevelAreaModule.View;

namespace Catzilla.LevelAreaModule.Model {
    public class LevelAreaGenerator {
        [Inject]
        public ObjectTypeInfoStorage ObjectTypeInfoStorage {get; set;}

        [Inject]
        public EnvTypeInfoStorage EnvTypeInfoStorage {get; set;}

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
            LevelObjectType[] objectTypesToSpawn =
                envTypeInfo.SpawnMap.GetObjectTypes();
            Array.Sort(objectTypesToSpawn, ObjectTypesComparator);
            LevelAreaView area = outputLevel.NewArea(envTypeInfo);

            for (int i = 0; i < objectTypesToSpawn.Length; ++i) {
                NewObjects(
                    objectTypesToSpawn[i],
                    envTypeInfo,
                    spawnPlayer,
                    area.Index,
                    outputLevel);
                yield return null;
            }

            if (onDone != null) {
                onDone(area);
            }
        }

        private void NewObjects(
            LevelObjectType objectType,
            EnvTypeInfo envTypeInfo,
            bool spawnPlayer,
            int areaIndex,
            LevelView outputLevel) {
            List<Bounds> spawnLocations =
                envTypeInfo.SpawnMap.GetLocations(objectType);
            int objectsCountToSpawn;
            ObjectTypeInfo objectTypeInfo =
                ObjectTypeInfoStorage.Get(objectType);

            if (spawnPlayer && objectType == PlayerObjectType) {
                objectsCountToSpawn = 1;
                spawnPlayer = false;
            } else {
                objectsCountToSpawn =
                    GetObjectsCountToSpawn(objectTypeInfo, outputLevel.Index);
            }

            Vector3 spawnPoint;
            int triesCount = objectsCountToSpawn * 2;

            for (int j = 0; j < objectsCountToSpawn; ++j) {
                do {
                    spawnPoint =
                        GetRandomSpawnPoint(spawnLocations, objectTypeInfo);
                    --triesCount;
                } while (IsSpawnPointReserved(spawnPoint) && triesCount > 0);

                if (triesCount == 0) {
                    break;
                }

                outputLevel.NewObject(objectTypeInfo, spawnPoint, areaIndex);
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

        private int GetObjectsCountToSpawn(
            ObjectTypeInfo objectTypeInfo, int levelIndex) {
            if (objectTypeInfo.SpawnChance == 0f
                || objectTypeInfo.SpawnChance < UnityEngine.Random.value) {
                return 0;
            }

            return objectTypeInfo.SpawnBaseCount +
                (int) (levelIndex * objectTypeInfo.SpawnLevelFactor);
        }

        private int ObjectTypesComparator(
            LevelObjectType lhs, LevelObjectType rhs) {
            return ObjectTypeInfoStorage.Get(lhs).SpawnPriority.CompareTo(
                ObjectTypeInfoStorage.Get(rhs).SpawnPriority);
        }
    }
}
