using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;
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
            activatable.Activate();
        }
    }
}
