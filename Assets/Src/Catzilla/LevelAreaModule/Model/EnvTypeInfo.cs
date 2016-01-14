using UnityEngine;
using System.Collections.Generic;
using Catzilla.LevelAreaModule.View;
using Catzilla.LevelObjectModule.Model;

namespace Catzilla.LevelAreaModule.Model {
    public class EnvTypeInfo {
        public EnvView ViewProto {get; private set;}

        private readonly IDictionary<LevelObjectType, List<LevelAreaRect>> spawnLocations;

        public EnvTypeInfo(
            EnvView viewProto,
            IDictionary<LevelObjectType, List<LevelAreaRect>> spawnLocations) {
            ViewProto = viewProto;
            this.spawnLocations = spawnLocations;
        }

        public LevelObjectType[] GetObjectTypes() {
            var objectTypesCollection = spawnLocations.Keys;
            var objectTypesArray =
                new LevelObjectType[objectTypesCollection.Count];
            objectTypesCollection.CopyTo(objectTypesArray, 0);
            return objectTypesArray;
        }

        public List<LevelAreaRect> GetSpawnLocations(
            LevelObjectType objectType) {
            return spawnLocations[objectType];
        }
    }
}
