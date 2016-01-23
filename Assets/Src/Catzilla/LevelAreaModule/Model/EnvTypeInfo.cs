using UnityEngine;
using System.Collections.Generic;
using Catzilla.LevelAreaModule.View;
using Catzilla.LevelObjectModule.Model;

namespace Catzilla.LevelAreaModule.Model {
    [System.Serializable]
    public class EnvTypeInfo {
        public EnvType Type;
        public LevelAreaView ViewProto;
        public int SpawnWeight = 1;

        public IDictionary<LevelObjectType, List<LevelAreaRect>> SpawnLocations {
            get {
                return spawnLocations;
            }
            set {
                spawnLocations = value;
                objectTypes = null;
            }
        }

        [System.NonSerialized]
        private LevelObjectType[] objectTypes;

        [System.NonSerialized]
        private IDictionary<LevelObjectType, List<LevelAreaRect>> spawnLocations;

        public LevelObjectType[] GetObjectTypes() {
            if (objectTypes == null) {
                var objectTypesCollection = spawnLocations.Keys;
                objectTypes =
                    new LevelObjectType[objectTypesCollection.Count];
                objectTypesCollection.CopyTo(objectTypes, 0);
            }

            return objectTypes;
        }

        public List<LevelAreaRect> GetObjectSpawnLocations(
            LevelObjectType objectType) {
            return spawnLocations[objectType];
        }
    }
}
