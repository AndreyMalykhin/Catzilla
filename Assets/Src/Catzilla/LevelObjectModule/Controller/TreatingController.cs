using UnityEngine;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.LevelObjectModule.View;

namespace Catzilla.LevelObjectModule.Controller {
    public class TreatingController {
        [Inject("PlayerMeshTag")]
        private string playerMeshTag;

        public void OnTriggerEnter(Evt evt) {
            var collider = (Collider) evt.Data;

            if (collider == null || !collider.CompareTag(playerMeshTag)) {
                return;
            }

            var player = collider.attachedRigidbody.GetComponent<PlayerView>();
            var treating = (TreatingView) evt.Source;
            player.Health += treating.AddHealth;
        }
    }
}
