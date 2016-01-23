using UnityEngine;
using System.Collections.Generic;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.Util;
using Catzilla.LevelObjectModule.View;

namespace Catzilla.LevelObjectModule.Controller {
    public class ShootingController {
        [Inject("PlayerFieldOfViewTag")]
        public string PlayerFieldOfViewTag {get; set;}

        public void OnTriggerEnter(IEvent evt) {
            var eventData = (EventData) evt.data;
            var collider = (Collider) eventData.Data;
            var shooter = (ShootingView) eventData.EventOwner;

            if (collider == null
                || !collider.CompareTag(PlayerFieldOfViewTag)) {
                return;
            }

            shooter.Target =
                collider.attachedRigidbody.GetComponent<PlayerView>().Collider;
        }
    }
}
