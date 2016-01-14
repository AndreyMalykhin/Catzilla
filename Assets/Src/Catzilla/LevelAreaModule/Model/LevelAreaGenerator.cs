using UnityEngine;
using System;
using System.Collections.Generic;
using Catzilla.LevelObjectModule.Model;

namespace Catzilla.LevelAreaModule.Model {
    public class LevelAreaGenerator {
        [Inject]
        public EnvFactory EnvFactory {get; set;}

        [Inject]
        public LevelObjectFactory ObjectFactory {get; set;}

        [Inject]
        public ObjectTypeInfoStorage ObjectTypeInfoStorage {get; set;}

        [Inject]
        public EnvTypeInfoStorage EnvTypeInfoStorage {get; set;}

        [Inject("PlayerObjectType")]
        public LevelObjectType PlayerObjectType {get; set;}

        private readonly IDictionary<LevelAreaPoint, bool> reservedSpawnPoints =
            new Dictionary<LevelAreaPoint, bool>();
        private int nextAreaIndex = 0;

        public LevelArea Generate(
            EnvType envType, int levelIndex, bool spawnPlayer) {
            Debug.Log("LevelAreaGenerator.Generate()");
            reservedSpawnPoints.Clear();
            LevelObjectType[] objectTypesToSpawn =
                EnvTypeInfoStorage.Get(envType).GetObjectTypes();
            Array.Sort(objectTypesToSpawn, (lhs, rhs) => {
                return ObjectTypeInfoStorage.Get(lhs).SpawnPriority.CompareTo(
                    ObjectTypeInfoStorage.Get(rhs).SpawnPriority);
            });
            Env env = EnvFactory.Make(envType);
            var objects = new List<LevelObject>();

            for (int i = 0; i < objectTypesToSpawn.Length; ++i) {
                SpawnObjects(objectTypesToSpawn[i], env, spawnPlayer,
                    levelIndex, objects);
            }

            return new LevelArea(nextAreaIndex++, env, objects);
        }

        private void SpawnObjects(
            LevelObjectType objectType,
            Env env,
            bool spawnPlayer,
            int levelIndex,
            List<LevelObject> outputObjects) {
            List<LevelAreaRect> spawnLocations =
                EnvTypeInfoStorage.Get(env.Type).GetSpawnLocations(objectType);
            int objectsCountToSpawn;
            ObjectTypeInfo objectTypeInfo =
                ObjectTypeInfoStorage.Get(objectType);

            if (spawnPlayer && objectType == PlayerObjectType) {
                objectsCountToSpawn = 1;
                spawnPlayer = false;
            } else {
                objectsCountToSpawn =
                    GetObjectsCountToSpawn(objectTypeInfo, levelIndex);
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

                var obj = ObjectFactory.Make(objectType);
                obj.SpawnPoint = spawnPoint;
                outputObjects.Add(obj);
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
