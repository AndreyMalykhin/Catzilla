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

            if (collider.CompareTag(PlayerTag)) {
                var player =
                    collider.attachedRigidbody.GetComponent<PlayerView>();
                int damage = ((DamagingView) eventData.EventOwner).Damage;
                player.TakeDamage(damage);
            }
        }
    }
}
