using UnityEngine;
using System;
using System.Collections;

namespace Catzilla.PlayerModule.Model {
    public class PlayerSettingsStorage {
        private PlayerSettings currentPlayerSettings = new PlayerSettings();

        public void GetCurrent(Action<PlayerSettings> onDone) {
            onDone(currentPlayerSettings);
        }

        public void Update(PlayerSettings settings, Action onDone) {
            currentPlayerSettings = settings;
            onDone();
        }
    }
}
