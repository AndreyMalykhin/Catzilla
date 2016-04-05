using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.Model;
using Catzilla.LevelModule.Model;
using Catzilla.LevelModule.View;
using Catzilla.LevelObjectModule.View;

namespace Catzilla.LevelObjectModule.Controller {
    public class ActivatableController {
        [Inject("PlayerFieldOfViewTag")]
        private string playerFieldOfViewTag;

        public void OnTriggerEnter(Evt evt) {
            var collider = (Collider) evt.Data;

            if (collider == null
                || !collider.CompareTag(playerFieldOfViewTag)) {
                return;
            }

            var activatable = (ActivatableView) evt.Source;
            activatable.IsActive = true;
        }

        public void OnLevelGenerate(Evt evt) {
            var level = (LevelView) evt.Data;
            var activatables = level.GetComponentsInChildren<ActivatableView>();

            for (int i = 0; i < activatables.Length; ++i) {
                activatables[i].IsActive = true;
            }
        }
    }
}
