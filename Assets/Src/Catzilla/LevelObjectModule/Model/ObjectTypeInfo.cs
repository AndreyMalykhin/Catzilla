using UnityEngine;
using System.Collections;
using Catzilla.LevelObjectModule.View;

namespace Catzilla.LevelObjectModule.Model {
    [System.Serializable]
    public class ObjectTypeInfo {
        public int Width;
        public int Depth;
        public int SpawnPriority;
        public float SpawnChance;
        public int SpawnBaseCount;
        public float SpawnLevelFactor;
        public LevelObjectView ViewProto;
    }
}
