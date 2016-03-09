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
        public float SpawnChance {get {return spawnChance;}}
        public int MinSpawnsPerArea {get {return minSpawnsPerArea;}}
        public int MaxSpawnsPerArea {get {return maxSpawnsPerArea;}}
        public float SpawnsCountLevelFactor {
            get {return spawnsCountLevelFactor;}
        }
        public bool IsSpawnsCountRandom {get {return isSpawnsCountRandom;}}
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
        private float spawnChance = 1f;

        [SerializeField]
        private int minSpawnsPerArea;

        [SerializeField]
        private int maxSpawnsPerArea;

        [SerializeField]
        private float spawnsCountLevelFactor;

        [SerializeField]
        private bool isSpawnsCountRandom;

        [SerializeField]
        private ObjectProtoInfo[] protoInfos;

        public int GetSpawnsPerArea(int levelIndex) {
            return Mathf.Min(
                MinSpawnsPerArea + (int) (levelIndex * SpawnsCountLevelFactor),
                MaxSpawnsPerArea);
        }
    }
}
