using UnityEngine;
using System.Collections;
using System.Text;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;
using Catzilla.PlayerModule.Model;
using Catzilla.LevelObjectModule.View;

namespace Catzilla.LevelObjectModule.Controller {
    public class SmashingController {
        [Inject]
        private PlayerManager playerManager;

        public void OnTriggerEnter(Evt evt) {
            var collider = (Collider) evt.Data;
            Rigidbody colliderBody = collider.attachedRigidbody;

            if (collider == null || colliderBody == null) {
                return;
            }

            // DebugUtils.Log("SmashingController.OnTriggerEnter(); {0}", collider);
            var explosive = colliderBody.GetComponent<ExplosiveView>();
            var smashable = colliderBody.GetComponent<SmashableView>();

            if (smashable == null && explosive == null) {
                return;
            }

            var smashing = (SmashingView) evt.Source;
            var player = smashing.GetComponent<PlayerView>();

            if (player != null) {
                var scoreable = colliderBody.GetComponent<ScoreableView>();

                if (scoreable != null) {
                    playerManager.AddScore(player, scoreable);
                }
            }

            if (explosive != null) {
                if (explosive.isActiveAndEnabled) {
                    explosive.Explode();
                }

                return;
            }

            float smashForce = UnityEngine.Random.Range(
                smashing.MinSmashForce, smashing.MaxSmashForce);

            if (smashable.isActiveAndEnabled) {
                smashable.Smash(
                    smashForce,
                    smashing.SmashUpwardsModifier,
                    smashing.transform.position);
            }
        }
    }
}
