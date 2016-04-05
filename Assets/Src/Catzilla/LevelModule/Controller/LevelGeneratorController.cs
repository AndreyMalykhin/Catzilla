using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.LevelObjectModule.View;
using Catzilla.LevelAreaModule.View;
using Catzilla.LevelModule.Model;
using Catzilla.LevelModule.View;

namespace Catzilla.LevelModule.Controller {
    public class LevelGeneratorController {
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

        public void OnLevelAreaDestroy(Evt evt) {
            if (level == null) {
                return;
            }

            levelGenerator.NewArea(player, level);
        }

        public void OnPreLevelLoad(Evt evt) {
            levelGenerator.Stop();
        }

        public void OnPreLevelUnload(Evt evt) {
            levelGenerator.Stop();
        }
    }
}
