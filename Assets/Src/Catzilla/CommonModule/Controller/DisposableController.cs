using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;

namespace Catzilla.CommonModule.Controller {
    public class DisposableController {
        [Inject("PlayerFieldOfViewTag")]
        private string playerFieldOfViewTag;

        public void OnTriggerExit(Evt evt) {
            var collider = (Collider) evt.Data;
            // DebugUtils.Log(
            //     "DisposableController.OnTriggerExit(); collider={0}", collider);

            if (collider == null
                || !collider.CompareTag(playerFieldOfViewTag)) {
                return;
            }

            var disposable = (DisposableView) evt.Source;
            disposable.Dispose();
        }
    }
}
