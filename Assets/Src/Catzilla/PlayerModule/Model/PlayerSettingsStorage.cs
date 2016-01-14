using UnityEngine;
using System.Collections;

namespace Catzilla.PlayerModule.Model {
    public class PlayerSettingsStorage {
        private PlayerSettings currentPlayer;

        public PlayerSettings GetCurrent() {
            if (currentPlayer == null) {
                int level = 0;
                currentPlayer = new PlayerSettings(level);
            }

            return currentPlayer;
        }
    }
}
