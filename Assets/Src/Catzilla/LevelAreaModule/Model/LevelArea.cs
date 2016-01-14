using UnityEngine;
using System.Collections.Generic;
using Catzilla.LevelObjectModule.Model;

namespace Catzilla.LevelAreaModule.Model {
    public struct LevelArea {
        public Env Env {get; private set;}
        public int Index {get; private set;}
        public List<LevelObject> Objects {get; private set;}

        public LevelArea(int index, Env env, List<LevelObject> objects) {
            Index = index;
            Env = env;
            Objects = objects;
        }
    }
}
