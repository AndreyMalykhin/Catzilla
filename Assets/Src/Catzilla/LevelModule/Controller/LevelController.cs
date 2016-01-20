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

        public void OnViewConstruct(IEvent evt) {
            PlayerSettingsStorage.GetCurrent((playerSettings) => {
                var msg = string.Format(Translator.GetTextValue(
                    "LevelStartScreen.Level"), playerSettings.Level + 1);
                LevelStartScreen.Msg.text = msg;
                LevelStartScreen.Show();
                var level = (LevelView) evt.data;
                level.gameObject.SetActive(false);
                LevelGenerator.NewLevel(playerSettings.Level, level);
            });
        }
    }
}
