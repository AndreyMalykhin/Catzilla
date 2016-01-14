using UnityEngine;
using System.Collections;

namespace Catzilla.LevelObjectModule.Model {
    public class LevelObjectFactory {
        public LevelObject Make(LevelObjectType type) {
            return new LevelObject(type);
        }
    }
}
