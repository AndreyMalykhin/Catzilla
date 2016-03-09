using UnityEngine;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.LevelObjectModule.View;

namespace Catzilla.LevelObjectModule.Controller {
    public class FleeingController {
        [Inject("ObjectComfortZoneTag")]
        private string objectComfortZoneTag;

        public void OnTriggerEnter(Evt evt) {
            var collider = (Collider) evt.Data;

            if (collider == null
                || !collider.CompareTag(objectComfortZoneTag)) {
                return;
            }

            var fleeing = (FleeingView) evt.Source;
            var otherFleeing =
                collider.attachedRigidbody.GetComponent<FleeingView>();

            if (fleeing.transform.position.z >
                    otherFleeing.transform.position.z) {
                return;
            }

            // DebugUtils.Log("FleeingController.OnTriggerEnter()");
            fleeing.Speed = 0f;
        }

        public void OnTriggerExit(Evt evt) {
            var collider = (Collider) evt.Data;

            if (collider == null
                || !collider.CompareTag(objectComfortZoneTag)) {
                return;
            }

            var fleeing = (FleeingView) evt.Source;
            var otherFleeing =
                collider.attachedRigidbody.GetComponent<FleeingView>();

            if (fleeing.transform.position.z >
                    otherFleeing.transform.position.z) {
                return;
            }

            // DebugUtils.Log("FleeingController.OnTriggerExit()");
            fleeing.Speed = 0.95f * otherFleeing.Speed;
        }
    }
}
