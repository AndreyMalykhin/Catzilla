using UnityEngine;
using System.Collections;

namespace Catzilla.PlayerModule.Model {
    public class PlayerSettingsStorage {
        private PlayerSettings currentPlayerSettings = new PlayerSettings();

        public PlayerSettings GetCurrent() {
            return currentPlayerSettings;
        }

        public void Update(PlayerSettings settings) {
            currentPlayerSettings = settings;
        }

        public void Sync() {}
    }
}
