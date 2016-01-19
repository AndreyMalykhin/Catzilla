using UnityEngine;
using System;
using System.Collections.Generic;
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

        private readonly IDictionary<LevelAreaPoint, bool> reservedSpawnPoints =
            new Dictionary<LevelAreaPoint, bool>();
        private int nextAreaIndex = 0;

        public void NewArea(
            EnvType envType, bool spawnPlayer, LevelView outputLevel) {
            reservedSpawnPoints.Clear();
            LevelObjectType[] objectTypesToSpawn =
                EnvTypeInfoStorage.Get(envType).GetObjectTypes();
            Array.Sort(objectTypesToSpawn, (lhs, rhs) => {
                return ObjectTypeInfoStorage.Get(lhs).SpawnPriority.CompareTo(
                    ObjectTypeInfoStorage.Get(rhs).SpawnPriority);
            });
            LevelAreaView area = outputLevel.NewArea(nextAreaIndex);
            EnvTypeInfo envTypeInfo = EnvTypeInfoStorage.Get(envType);
            area.NewEnv(envTypeInfo);

            for (int i = 0; i < objectTypesToSpawn.Length; ++i) {
                NewObjects(
                    objectTypesToSpawn[i],
                    envType,
                    spawnPlayer,
                    nextAreaIndex,
                    outputLevel);
            }

            ++nextAreaIndex;
        }

        private void NewObjects(
            LevelObjectType objectType,
            EnvType envType,
            bool spawnPlayer,
            int areaIndex,
            LevelView outputLevel) {
            List<LevelAreaRect> spawnLocations = EnvTypeInfoStorage.Get(envType)
                .GetObjectSpawnLocations(objectType);
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

            LevelAreaPoint spawnPoint;
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

        private LevelAreaPoint GetRandomSpawnPoint(
            List<LevelAreaRect> spawnLocations, ObjectTypeInfo objectTypeInfo) {
            var spawnLocation = spawnLocations[
                UnityEngine.Random.Range(0, spawnLocations.Count)];
            Debug.Assert(spawnLocation.Width >= objectTypeInfo.Width);
            Debug.Assert(spawnLocation.Depth >= objectTypeInfo.Depth);
            int x = UnityEngine.Random.Range(
                0, spawnLocation.Width / objectTypeInfo.Width);
            int z = UnityEngine.Random.Range(
                0, spawnLocation.Depth / objectTypeInfo.Depth);
            return new LevelAreaPoint(
                spawnLocation.Start.X + x * objectTypeInfo.Width,
                spawnLocation.Start.Z + z * objectTypeInfo.Depth);
        }

        private bool IsSpawnPointReserved(LevelAreaPoint point) {
            return reservedSpawnPoints.ContainsKey(point);
        }

        private void ReserveSpawnPoint(LevelAreaPoint point) {
            reservedSpawnPoints[new LevelAreaPoint(point.X, point.Z)] = true;
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
    }
}
