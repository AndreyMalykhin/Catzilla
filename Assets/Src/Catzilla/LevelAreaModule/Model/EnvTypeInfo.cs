using UnityEngine;
using System.Collections.Generic;
using Catzilla.LevelAreaModule.View;
using Catzilla.LevelObjectModule.Model;

namespace Catzilla.LevelAreaModule.Model {
    [System.Serializable]
    public class EnvTypeInfo {
        public EnvType Type;
        public EnvView ViewProto;
        public int SpawnWeight = 1;

        public IDictionary<LevelObjectType, List<LevelAreaRect>> SpawnLocations {get; set;}

        [System.NonSerialized]
        private LevelObjectType[] objectTypes;

        public LevelObjectType[] GetObjectTypes() {
            if (objectTypes == null) {
                var objectTypesCollection = SpawnLocations.Keys;
                objectTypes =
                    new LevelObjectType[objectTypesCollection.Count];
                objectTypesCollection.CopyTo(objectTypes, 0);
            }

            return objectTypes;
        }

        public List<LevelAreaRect> GetObjectSpawnLocations(
            LevelObjectType objectType) {
            return SpawnLocations[objectType];
        }
    }
}
