using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.LevelObjectModule.View;
using Catzilla.LevelModule.Model;
using Catzilla.LevelModule.View;
using Catzilla.LevelAreaModule.View;

namespace Catzilla.LevelAreaModule.Controller {
    public class LevelAreaController {
        [Inject]
        private LevelGenerator levelGenerator;

        private LevelView level;
        private PlayerView player;

        public void OnLevelConstruct(Evt evt) {
            level = (LevelView) evt.Source;
        }

        public void OnPlayerConstruct(Evt evt) {
            player = (PlayerView) evt.Source;
        }

        public void OnDestroy(Evt evt) {
            if (level == null) {
                return;
            }

            levelGenerator.NewArea(player, level);
        }
    }
}
