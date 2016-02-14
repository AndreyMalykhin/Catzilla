using UnityEngine;
using System;
using System.Collections.Generic;
using Catzilla.LevelAreaModule.View;
using Catzilla.LevelObjectModule.Model;

namespace Catzilla.LevelAreaModule.Model {
    [Serializable]
    public class EnvTypeInfo {
        public EnvType Type {get {return type;}}
        public LevelAreaView ViewProto {get {return viewProto;}}
        public int SpawnWeight {get {return spawnWeight;}}
        public SpawnMapView SpawnMap {get {return spawnMap;}}

        [SerializeField]
        private EnvType type;

        [SerializeField]
        private LevelAreaView viewProto;

        [SerializeField]
        private int spawnWeight = 1;

        [SerializeField]
        private SpawnMapView spawnMap;
    }
}
