using UnityEngine;
using System.Collections;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.Util;
using Catzilla.LevelObjectModule.View;

namespace Catzilla.LevelObjectModule.Controller {
    public class ActivatableController {
        [Inject("PlayerFieldOfViewTag")]
        public string PlayerFieldOfViewTag {get; set;}

        public void OnTriggerEnter(Evt evt) {
            var collider = (Collider) evt.Data;

            if (collider == null
                || !collider.CompareTag(PlayerFieldOfViewTag)) {
                return;
            }

            var activatable = (ActivatableView) evt.Source;
            activatable.Activate();
        }
    }
}
