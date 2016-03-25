using UnityEngine;
using UnityEngine.Serialization;
using System;
using System.Collections;
using Catzilla.LevelObjectModule.View;

namespace Catzilla.LevelObjectModule.Model {
    [Serializable]
    public class ObjectTypeInfo {
        public LevelObjectType Type {get {return type;}}
        public int Width {get {return width;}}
        public int Depth {get {return depth;}}
        public int SpawnPriority {get {return spawnPriority;}}
        public ObjectProtoInfo ProtoInfo {get {return protoInfos[0];}}
        public ObjectProtoInfo[] ProtoInfos {get {return protoInfos;}}

        [SerializeField]
        private LevelObjectType type;

        [SerializeField]
        private int width;

        [SerializeField]
        private int depth;

        [SerializeField]
        private int spawnPriority;

        [SerializeField]
        private ObjectProtoInfo[] protoInfos;
    }
}
