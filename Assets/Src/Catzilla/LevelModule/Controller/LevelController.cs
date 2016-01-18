using UnityEngine;
using System.Collections;
using strange.extensions.dispatcher.eventdispatcher.api;
using SmartLocalization;
using Catzilla.CommonModule.Util;
using Catzilla.PlayerModule.Model;
using Catzilla.LevelAreaModule.View;
using Catzilla.LevelModule.Model;
using Catzilla.LevelModule.View;

namespace Catzilla.LevelModule.Controller {
    public class LevelController {
        [Inject]
        public LevelGenerator LevelGenerator {get; set;}

        [Inject]
        public PlayerSettingsStorage PlayerSettingsStorage {get; set;}

        [Inject]
        public LevelStartScreenView LevelStartScreen {get; set;}

        [Inject]
        public LanguageManager Translator {get; set;}

        [Inject("PlayerMeshTag")]
        public string PlayerTag {get; set;}

        [Inject("MainCamera")]
        public Camera MainCamera {get; set;}

        private LevelView level;

        public void OnViewReady(IEvent evt) {
            PlayerSettingsStorage.GetCurrent((playerSettings) => {
                var msg = string.Format(Translator.GetTextValue(
                    "LevelStartScreen.Level"), playerSettings.Level + 1);
                LevelStartScreen.Msg.text = msg;
                LevelStartScreen.Show();
                level = (LevelView) evt.data;
                level.gameObject.SetActive(false);
                LevelGenerator.NewLevel(playerSettings.Level, level);
            });
        }

        public void OnStartScreenHide() {
            level.gameObject.SetActive(true);
            MainCamera.gameObject.SetActive(false);
        }

        public void OnAreaTriggerEnter(IEvent evt) {
            if (((Collider) evt.data).CompareTag(PlayerTag)) {
                 LevelGenerator.NewArea(level);
            }
        }
    }
}
