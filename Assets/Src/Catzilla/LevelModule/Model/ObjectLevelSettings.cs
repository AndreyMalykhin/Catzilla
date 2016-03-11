using UnityEngine;
using System;
using Catzilla.LevelObjectModule.Model;

namespace Catzilla.LevelModule.Model {
    [Serializable]
    public class ObjectLevelSettings {
        public LevelObjectType ObjectType {get {return objectType;}}
        public int MinSpawnsPerArea {get {return minSpawnsPerArea;}}
        public int MaxSpawnsPerArea {get {return maxSpawnsPerArea;}}

        [SerializeField]
        private LevelObjectType objectType;

        [SerializeField]
        private int minSpawnsPerArea;

        [SerializeField]
        private int maxSpawnsPerArea;
    }
}
