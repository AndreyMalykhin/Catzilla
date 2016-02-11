using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.LevelObjectModule.View;

namespace Catzilla.LevelObjectModule.Controller {
    public class DamagingController {
        [Inject("PlayerMeshTag")]
        public string PlayerTag {get; set;}

        public void OnTriggerEnter(Evt evt) {
            var collider = (Collider) evt.Data;

            if (collider == null || !collider.CompareTag(PlayerTag)) {
                return;
            }

            var player =
                collider.attachedRigidbody.GetComponent<PlayerView>();
            var damager = (DamagingView) evt.Source;
            player.Health -= damager.Damage;
        }
    }
}
