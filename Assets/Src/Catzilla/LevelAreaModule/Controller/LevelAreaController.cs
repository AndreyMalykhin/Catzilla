using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.LevelModule.Model;
using Catzilla.LevelModule.View;
using Catzilla.LevelAreaModule.View;

namespace Catzilla.LevelAreaModule.Controller {
    public class LevelAreaController {
        [Inject]
        public LevelGenerator LevelGenerator {get; set;}

        [Inject("PlayerFieldOfViewTag")]
        public string PlayerFieldOfViewTag {get; set;}

        private LevelView level;

        public void OnLevelConstruct(Evt evt) {
            level = (LevelView) evt.Source;
        }

        public void OnTriggerEnter(Evt evt) {
            var collider = (Collider) evt.Data;

            if (collider == null
                || !collider.CompareTag(PlayerFieldOfViewTag)) {
                return;
            }

            var area = (LevelAreaView) evt.Source;

            if (area.Index < LevelGenerator.InitialAreasCount - 1) {
                return;
            }

            LevelGenerator.NewArea(level);
        }
    }
}
