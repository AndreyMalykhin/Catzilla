using UnityEngine;
using System.Collections.Generic;
using Catzilla.CommonModule.Util;
using Catzilla.LevelObjectModule.Model;

namespace Catzilla.LevelAreaModule.View {
    public class SpawnMapView: MonoBehaviour {
        [SerializeField]
        private SpawnView[] spawns;

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
