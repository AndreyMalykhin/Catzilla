using UnityEngine;
using System.Collections;
using Catzilla.LevelAreaModule.Model;

namespace Catzilla.LevelObjectModule.Model {
    public class LevelObject {
        public LevelObjectType Type {get; private set;}
        public LevelAreaPoint SpawnPoint {get; set;}

        public LevelObject(LevelObjectType type) {
            Type = type;
        }
    }
}
