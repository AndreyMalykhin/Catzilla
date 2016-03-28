using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.Model;
using Catzilla.LevelObjectModule.View;

namespace Catzilla.LevelObjectModule.Controller {
    public class ActivatableController {
        [Inject("PlayerFieldOfViewTag")]
        private string playerFieldOfViewTag;

        [Inject]
        private Game game;

        public void OnTriggerEnter(Evt evt) {
            var collider = (Collider) evt.Data;

            if (collider == null
                || !collider.CompareTag(playerFieldOfViewTag)) {
                return;
            }

            var activatable = (ActivatableView) evt.Source;
            activatable.IsActive = true;
        }

        public void OnConstruct(Evt evt) {
            if (game.IsPaused) {
                var activatable = (ActivatableView) evt.Source;
                activatable.IsActive = true;
            }
        }
    }
}
