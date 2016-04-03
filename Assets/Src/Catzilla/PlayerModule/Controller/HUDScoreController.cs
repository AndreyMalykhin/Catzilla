using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.LevelObjectModule.View;
using Catzilla.LevelModule.View;
using Catzilla.LevelModule.Model;
using Catzilla.PlayerModule.View;
using Catzilla.PlayerModule.Model;

namespace Catzilla.PlayerModule.Controller {
    public class HUDScoreController {
        [Inject]
        public Translator Translator {get; set;}

        [Inject]
        public LevelSettingsStorage LevelSettingsStorage {get; set;}

        [Inject]
        public PlayerStateStorage PlayerStateStorage {get; set;}

        private HUDScoreView score;

        public void OnViewConstruct(Evt evt) {
            score = (HUDScoreView) evt.Source;
            int level = PlayerStateStorage.Get().Level;
            score.MaxValue =
                LevelSettingsStorage.Get(level).CompletionScore;
        }

        public void OnChange(Evt evt) {
            var player = (PlayerView) evt.Source;
            score.Value = player.Score;
        }
    }
}
