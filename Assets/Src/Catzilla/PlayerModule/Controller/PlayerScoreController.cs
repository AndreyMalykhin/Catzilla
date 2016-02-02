using UnityEngine;
using System.Collections;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.Util;
using Catzilla.LevelObjectModule.View;
using Catzilla.LevelModule.View;
using Catzilla.LevelModule.Model;
using Catzilla.PlayerModule.View;

namespace Catzilla.PlayerModule.Controller {
    public class PlayerScoreController {
        [Inject]
        public Translator Translator {get; set;}

        [Inject]
        public LevelSettingsStorage LevelSettingsStorage {get; set;}

        private PlayerScoreView score;
        private LevelView level;

        public void OnLevelConstruct(Evt evt) {
            level = (LevelView) evt.Source;
        }

        public void OnViewConstruct(Evt evt) {
            score = (PlayerScoreView) evt.Source;
            score.Text = Translator.Translate("Player.Score");
            score.MaxValue =
                LevelSettingsStorage.Get(level.Index).CompletionScore;
        }

        public void OnChange(Evt evt) {
            var player = (PlayerView) evt.Source;
            score.Value = player.Score;
        }
    }
}
