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

        [Inject("PlayerFieldOfViewTag")]
        private string playerFieldOfViewTag;

        private LevelView level;
        private PlayerView player;

        public void OnLevelConstruct(Evt evt) {
            level = (LevelView) evt.Source;
        }

        public void OnPlayerConstruct(Evt evt) {
            player = (PlayerView) evt.Source;
        }

        public void OnTriggerEnter(Evt evt) {
            var collider = (Collider) evt.Data;

            if (collider == null
                || !collider.CompareTag(playerFieldOfViewTag)) {
                return;
            }

            var area = (LevelAreaView) evt.Source;

            if (area.Index < levelGenerator.InitialAreasCount - 1) {
                return;
            }

            levelGenerator.NewArea(player, level);
        }
    }
}
