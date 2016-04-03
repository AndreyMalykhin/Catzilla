using UnityEngine;
using System;
using System.Collections.Generic;
using Catzilla.CommonModule.Util;
using Catzilla.LevelObjectModule.Model;
using Catzilla.LevelAreaModule.View;

namespace Catzilla.LevelAreaModule.Model {
    [CreateAssetMenuAttribute]
    public class SpawnMap: ScriptableObject {
        public LevelObjectType[] ObjectTypes {get {return objectTypes;}}

        [SerializeField]
        private SpawnMapView viewProto;

        [NonSerialized]
        private readonly IDictionary<int, List<SpawnLocation>> items =
            new Dictionary<int, List<SpawnLocation>>(32);

        [NonSerialized]
        private LevelObjectType[] objectTypes;

        public List<SpawnLocation> GetLocations(LevelObjectType objectType) {
            return items[(int) objectType];
        }

        private void OnEnable() {
            // DebugUtils.Log("SpawnMap.OnEnable()");
            DebugUtils.Assert(items.Count == 0);
            DebugUtils.Assert(objectTypes == null);
            SpawnView[] spawns = viewProto.Items;

            for (int i = 0; i < spawns.Length; ++i) {
                SpawnView spawn = spawns[i];
                List<LevelObjectType> spawnObjectTypes = spawn.ObjectTypes;

                for (int j = 0; j < spawnObjectTypes.Count; ++j) {
                    List<SpawnLocation> spawnLocations = null;

                    if (!items.TryGetValue(
                            (int) spawnObjectTypes[j], out spawnLocations)) {
                        spawnLocations = new List<SpawnLocation>(64);
                        items[(int) spawnObjectTypes[j]] = spawnLocations;
                    }

                    spawnLocations.Add(spawn.Location);
                }
            }

            var objectTypesCollection = items.Keys;
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
