using UnityEngine;
using System.Collections.Generic;
using Catzilla.CommonModule.Util;
using Catzilla.LevelObjectModule.Model;
using Catzilla.LevelAreaModule.Model;

namespace Catzilla.LevelAreaModule.View {
    [ExecuteInEditMode]
    public class SpawnMapView: MonoBehaviour {
        [SerializeField]
        private SpawnView[] spawns;

        [SerializeField]
        private int width = 16;

        [SerializeField]
        private int depth = 24;

        private IDictionary<int, List<SpawnLocation>> spawnsMap;
        private LevelObjectType[] objectTypes;

        public LevelObjectType[] GetObjectTypes() {
            EnsureCache();
            return objectTypes;
        }

        public List<SpawnLocation> GetLocations(LevelObjectType objectType) {
            EnsureCache();
            return spawnsMap[(int) objectType];
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.grey;

            for (int x = -width / 2; x <= width / 2; ++x) {
                Gizmos.DrawLine(
                    new Vector3(x, 0f, -depth / 2),
                    new Vector3(x, 0f, depth / 2));
            }

            for (int z = -depth / 2; z <= depth / 2; ++z) {
                Gizmos.DrawLine(
                    new Vector3(-width / 2, 0f, z),
                    new Vector3(width / 2, 0f, z));
            }
        }

        private void EnsureCache() {
            if (spawnsMap != null) {
                return;
            }

            spawnsMap = new Dictionary<int, List<SpawnLocation>>();

            for (int i = 0; i < spawns.Length; ++i) {
                SpawnView spawn = spawns[i];
                List<LevelObjectType> spawnObjectTypes = spawn.ObjectTypes;

                for (int j = 0; j < spawnObjectTypes.Count; ++j) {
                    List<SpawnLocation> spawnLocations = null;

                    if (!spawnsMap.TryGetValue(
                            (int) spawnObjectTypes[j], out spawnLocations)) {
                        spawnLocations = new List<SpawnLocation>(64);
                        spawnsMap[(int) spawnObjectTypes[j]] = spawnLocations;
                    }

                    spawnLocations.Add(spawn.Location);
                }
            }

            var objectTypesCollection = spawnsMap.Keys;
            objectTypes =
                new LevelObjectType[objectTypesCollection.Count];
            var k = 0;

            foreach (int objectType in objectTypesCollection) {
                objectTypes[k] = (LevelObjectType) objectType;
                ++k;
            }
        }
    }
}
