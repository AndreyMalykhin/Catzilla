using UnityEngine;
using System;
using System.Collections;
using Catzilla.LevelObjectModule.View;

namespace Catzilla.LevelObjectModule.Model {
    [Serializable]
    public class ObjectTypeInfo {
        public LevelObjectType Type;
        public int Width;
        public int Depth;
        public int SpawnPriority;
        public float SpawnChance;
        public int SpawnBaseCount;
        public float SpawnLevelFactor;
        public LevelObjectView ViewProto;
    }
}
