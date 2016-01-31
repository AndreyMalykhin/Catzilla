using UnityEngine;
using System.Collections;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;

namespace Catzilla.CommonModule.Controller {
    public class DisposableController {
        [Inject("PlayerFieldOfViewTag")]
        public string PlayerFieldOfViewTag {get; set;}

        public void OnTriggerExit(IEvent evt) {
            var eventData = (EventData) evt.data;
            var collider = (Collider) eventData.Data;
            // DebugUtils.Log(
            //     "DisposableController.OnTriggerExit(); collider={0}", collider);

            if (collider == null
                || !collider.CompareTag(PlayerFieldOfViewTag)) {
                return;
            }

            var disposable = (DisposableView) eventData.EventOwner;
            disposable.Dispose();
        }
    }
}
