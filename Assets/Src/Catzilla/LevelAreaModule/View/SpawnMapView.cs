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

        private IDictionary<LevelObjectType, List<SpawnLocation>> spawnsMap;
        private LevelObjectType[] objectTypes;

        public LevelObjectType[] GetObjectTypes() {
            EnsureCache();
            return objectTypes;
        }

        public List<SpawnLocation> GetLocations(LevelObjectType objectType) {
            EnsureCache();
            return spawnsMap[objectType];
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

            spawnsMap = new Dictionary<LevelObjectType, List<SpawnLocation>>();

            for (int i = 0; i < spawns.Length; ++i) {
                List<SpawnLocation> locations;
                SpawnView spawn = spawns[i];

                if (!spawnsMap.TryGetValue(spawn.ObjectType, out locations)) {
                    locations = new List<SpawnLocation>(64);
                    spawnsMap[spawn.ObjectType] = locations;
                }

                locations.Add(spawn.Location);
            }

            var objectTypesCollection = spawnsMap.Keys;
            objectTypes =
                new LevelObjectType[objectTypesCollection.Count];
            objectTypesCollection.CopyTo(objectTypes, 0);
        }
    }
}
