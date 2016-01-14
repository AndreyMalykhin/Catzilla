using UnityEngine;
using System.Collections.Generic;

namespace Catzilla.LevelAreaModule.Model {
    public class Env {
        public EnvType Type {get; private set;}

        public Env(EnvType type) {
            Type = type;
        }
    }
}
