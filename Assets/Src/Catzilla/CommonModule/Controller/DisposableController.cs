using UnityEngine;
using System.Collections;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;

namespace Catzilla.CommonModule.Controller {
    public class DisposableController {
        [Inject]
        public PoolStorageView PoolStorage {get; set;}

        [Inject("PlayerFieldOfViewTag")]
        public string PlayerFieldOfViewTag {get; set;}

        public void OnTriggerExit(IEvent evt) {
            var eventData = (EventData) evt.data;
            var collider = (Collider) eventData.Data;
            // Debug.LogFormat(
            //     "DisposableController.OnTriggerExit(); collider={0}", collider);

            if (collider == null
                || !collider.CompareTag(PlayerFieldOfViewTag)) {
                return;
            }

            Dispose((DisposableView) eventData.EventOwner);
        }

        private void Dispose(DisposableView disposable) {
            GameObject root = disposable.Root == null ? disposable.gameObject :
                disposable.Root;
            PoolStorage.Return(root.GetComponent<PoolableView>());
        }
    }
}
