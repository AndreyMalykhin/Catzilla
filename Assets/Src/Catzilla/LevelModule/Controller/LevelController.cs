using UnityEngine;
using System.Collections;
using strange.extensions.dispatcher.eventdispatcher.api;
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
        public PlayerStateStorage PlayerStateStorage {get; set;}

        [Inject]
        public LevelStartScreenView LevelStartScreen {get; set;}

        [Inject]
        public Translator Translator {get; set;}

        public void OnViewConstruct(IEvent evt) {
            var level = (LevelView) evt.data;
            level.gameObject.SetActive(false);
            PlayerState playerState = PlayerStateStorage.Get();
            LevelGenerator.NewLevel(playerState.Level, level);
            var msg = string.Format(Translator.Translate(
                "LevelStartScreen.Level"), playerState.Level + 1);
            LevelStartScreen.Msg.text = msg;
            LevelStartScreen.Show();
        }
    }
}
