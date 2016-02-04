using UnityEngine;
using System.Collections.Generic;
using Catzilla.CommonModule.Util;
using Catzilla.LevelObjectModule.Model;

namespace Catzilla.LevelAreaModule.View {
    [ExecuteInEditMode]
    public class SpawnMapView: MonoBehaviour {
        [SerializeField]
        private SpawnView[] spawns;

        [SerializeField]
        private int width = 16;

        [SerializeField]
        private int depth = 24;

        private IDictionary<LevelObjectType, List<Bounds>> spawnsMap;
        private LevelObjectType[] objectTypes;

        public LevelObjectType[] GetObjectTypes() {
            EnsureCache();
            return objectTypes;
        }

        public List<Bounds> GetLocations(LevelObjectType objectType) {
            EnsureCache();
            return spawnsMap[objectType];
        }

        public void OnDrawGizmos() {
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

            spawnsMap = new Dictionary<LevelObjectType, List<Bounds>>();

            for (int i = 0; i < spawns.Length; ++i) {
                List<Bounds> locations;
                SpawnView spawn = spawns[i];

                if (!spawnsMap.TryGetValue(spawn.ObjectType, out locations)) {
                    locations = new List<Bounds>(64);
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
