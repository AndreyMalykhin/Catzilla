using UnityEngine;
using System.Collections;
using SmartLocalization;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.LevelObjectModule.View;
using Catzilla.PlayerModule.View;

namespace Catzilla.PlayerModule.Controller {
    public class PlayerScoreController {
        [Inject]
        public LanguageManager Translator {get; set;}

        private PlayerScoreView score;

        public void OnViewReady(IEvent evt) {
            score = (PlayerScoreView) evt.data;
            score.Text = Translator.GetTextValue("Player.Score");
        }

        public void OnChange(IEvent evt) {
            var player = (PlayerView) evt.data;
            score.Value = player.Score;
        }
    }
}
