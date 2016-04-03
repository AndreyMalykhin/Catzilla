using UnityEngine;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.LevelObjectModule.View;

namespace Catzilla.LevelObjectModule.Controller {
    public class FleeingController {
        [Inject("PlayerFieldOfViewTag")]
        private string playerFieldOfViewTag;

        public void OnTriggerEnter(Evt evt) {
            var collider = (Collider) evt.Data;

            if (collider == null
                || !collider.CompareTag(playerFieldOfViewTag)) {
                return;
            }

            // DebugUtils.Log("FleeingController.OnTriggerEnter()");
            var fleeing = (FleeingView) evt.Source;
            fleeing.Danger = collider.attachedRigidbody.transform;
        }
    }
}
