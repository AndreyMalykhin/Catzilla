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
        public LevelObjectView ViewProto;
        public Material[] AvailableMaterials;
    }
}
