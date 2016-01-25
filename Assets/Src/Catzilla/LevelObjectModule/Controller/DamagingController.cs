using UnityEngine;
using System.Collections;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.Util;
using Catzilla.LevelObjectModule.View;

namespace Catzilla.LevelObjectModule.Controller {
    public class DamagingController {
        [Inject("PlayerMeshTag")]
        public string PlayerTag {get; set;}

        public void OnTriggerEnter(IEvent evt) {
            var eventData = (EventData) evt.data;
            var collider = (Collider) eventData.Data;

            if (collider == null || !collider.CompareTag(PlayerTag)) {
                return;
            }

            var player =
                collider.attachedRigidbody.GetComponent<PlayerView>();
            var damager = (DamagingView) eventData.EventOwner;
            player.Health -= damager.Damage;
        }
    }
}
