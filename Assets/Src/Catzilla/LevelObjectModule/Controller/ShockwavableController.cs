using UnityEngine;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.LevelObjectModule.View;

namespace Catzilla.LevelObjectModule.Controller {
    public class ShockwavableController {
        [Inject("PlayerMeshTag")]
        private string playerMeshTag;

        public void OnTriggerEnter(Evt evt) {
            var collider = (Collider) evt.Data;

            if (collider == null || !collider.CompareTag(playerMeshTag)) {
                return;
            }

            var shockwavable = (ShockwavableView) evt.Source;

            if (!shockwavable.IsTrigger
                || shockwavable.GetComponent<ExplosiveView>() != null) {
                return;
            }

            shockwavable.Propagate();
        }

        public void OnExplosion(Evt evt) {
            var explosive = (ExplosiveView) evt.Source;
            var shockwavable = explosive.GetComponent<ShockwavableView>();

            if (shockwavable == null || !shockwavable.IsTrigger) {
                return;
            }

            shockwavable.Propagate();
        }

        public void OnShot(Evt evt) {
            var shooting = (ShootingView) evt.Source;
            var shockwavable = shooting.GetComponent<ShockwavableView>();

            if (shockwavable == null || shockwavable.IsTrigger) {
                return;
            }

            shockwavable.Propagate();
        }
    }
}
