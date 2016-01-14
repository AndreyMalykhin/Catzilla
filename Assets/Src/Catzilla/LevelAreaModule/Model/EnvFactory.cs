using UnityEngine;
using System.Collections;

namespace Catzilla.LevelAreaModule.Model {
    public class EnvFactory {
        public Env Make(EnvType type) {
            return new Env(type);
        }
    }
}
