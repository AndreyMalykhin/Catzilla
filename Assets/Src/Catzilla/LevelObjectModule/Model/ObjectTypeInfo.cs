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
        public int MinSpawnsPerArea {get {return minSpawnsPerArea;}}
        public int MaxSpawnsPerArea {get {return maxSpawnsPerArea;}}
        public float SpawnsCountLevelFactor {
            get {return spawnsCountLevelFactor;}
        }
        public bool IsSpawnsCountRandom {get {return isSpawnsCountRandom;}}
        public LevelObjectView ViewProto {get {return viewProto;}}
        public Material[] AvailableMaterials {get {return availableMaterials;}}

        [SerializeField]
        private LevelObjectType type;

        [SerializeField]
        private int width;

        [SerializeField]
        private int depth;

        [SerializeField]
        private int spawnPriority;

        [SerializeField]
        private int minSpawnsPerArea;

        [SerializeField]
        private int maxSpawnsPerArea;

        [SerializeField]
        private float spawnsCountLevelFactor;

        [SerializeField]
        private bool isSpawnsCountRandom;

        [SerializeField]
        private LevelObjectView viewProto;

        [SerializeField]
        private Material[] availableMaterials;

        public int GetSpawnsPerArea(int levelIndex) {
            return Mathf.Min(
                MinSpawnsPerArea + (int) (levelIndex * SpawnsCountLevelFactor),
                MaxSpawnsPerArea);
        }
    }
}
