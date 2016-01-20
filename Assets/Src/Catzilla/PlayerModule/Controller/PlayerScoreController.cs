using UnityEngine;
using System.Collections;
using SmartLocalization;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.LevelObjectModule.View;
using Catzilla.LevelModule.View;
using Catzilla.LevelModule.Model;
using Catzilla.PlayerModule.View;

namespace Catzilla.PlayerModule.Controller {
    public class PlayerScoreController {
        [Inject]
        public LanguageManager Translator {get; set;}

        [Inject]
        public LevelSettingsStorage LevelSettingsStorage {get; set;}

        private PlayerScoreView score;
        private LevelView level;

        public void OnLevelConstruct(IEvent evt) {
            level = (LevelView) evt.data;
        }

        public void OnViewConstruct(IEvent evt) {
            score = (PlayerScoreView) evt.data;
            score.Text = Translator.GetTextValue("Player.Score");
            score.MaxValue =
                LevelSettingsStorage.Get(level.Index).CompletionScore;
        }

        public void OnChange(IEvent evt) {
            var player = (PlayerView) evt.data;
            score.Value = player.Score;
        }
    }
}
