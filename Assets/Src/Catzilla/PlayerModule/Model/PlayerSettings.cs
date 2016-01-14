using UnityEngine;
using System.Collections;

namespace Catzilla.PlayerModule.Model {
    public class PlayerSettings {
        public int Level {get; private set;}

        public PlayerSettings(int level) {
            Level = level;
        }
    }
}
