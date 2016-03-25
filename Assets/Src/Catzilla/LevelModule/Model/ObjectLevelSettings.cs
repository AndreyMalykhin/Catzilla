using UnityEngine;
using System;
using Catzilla.LevelObjectModule.Model;

namespace Catzilla.LevelModule.Model {
    [Serializable]
    public class ObjectLevelSettings {
        public LevelObjectType ObjectType {get {return objectType;}}
        public int MinSpawnsPerArea {get {return minSpawnsPerArea;}}
        public int MaxSpawnsPerArea {get {return maxSpawnsPerArea;}}
        public float SpawnChance {get {return spawnChance;}}

        [SerializeField]
        private LevelObjectType objectType;

        [SerializeField]
        private int minSpawnsPerArea;

        [SerializeField]
        private int maxSpawnsPerArea;

        [SerializeField]
        private float spawnChance = 1f;
    }
}
