using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.LevelObjectModule.View;

namespace Catzilla.LevelObjectModule.Controller {
    public class DamagingController {
        [Inject("PlayerMeshTag")]
        private string playerMeshTag;

        public void OnTriggerEnter(Evt evt) {
            var collider = (Collider) evt.Data;

            if (collider == null || !collider.CompareTag(playerMeshTag)) {
                return;
            }

            var player =
                collider.attachedRigidbody.GetComponent<PlayerView>();
            var damager = (DamagingView) evt.Source;
            player.Health -= damager.Damage;
        }
    }
}
