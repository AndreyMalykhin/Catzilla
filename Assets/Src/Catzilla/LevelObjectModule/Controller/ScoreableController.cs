using UnityEngine;
using System.Collections;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.Util;
using Catzilla.LevelObjectModule.View;

namespace Catzilla.LevelObjectModule.Controller {
    public class ScoreableController {
        [Inject("PlayerMeshTag")]
        public string PlayerTag {get; set;}

        public void OnTriggerEnter(IEvent evt) {
            var eventData = (EventData) evt.data;
            var collider = (Collider) eventData.Data;
            // DebugUtils.Log(
            //     "ScoreableController.OnTriggerEnter(); collider={0}", collider);

            if (collider != null && collider.CompareTag(PlayerTag)) {
                var player =
                    collider.attachedRigidbody.GetComponent<PlayerView>();
                player.Score += ((ScoreableView) eventData.EventOwner).Score;
            }
        }
    }
}
