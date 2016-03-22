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
            var colliderExplosive = colliderBody.GetComponent<ExplosiveView>();
            var colliderSmashable = colliderBody.GetComponent<SmashableView>();

            if (colliderSmashable == null && colliderExplosive == null) {
                return;
            }

            var smashing = (SmashingView) evt.Source;
            var player = smashing.GetComponent<PlayerView>();

            if (player != null) {
                var colliderScoreable =
                    colliderBody.GetComponent<ScoreableView>();

                if (colliderScoreable != null) {
                    playerManager.AddScore(player, colliderScoreable);
                }

                var colliderDamaging =
                    colliderBody.GetComponent<DamagingView>();

                if (colliderDamaging != null) {
                    player.Health -= colliderDamaging.Damage;
                }
            }

            if (colliderExplosive != null) {
                if (colliderExplosive.isActiveAndEnabled) {
                    colliderExplosive.Explode();
                }
            } else if (colliderSmashable != null
                && colliderSmashable.isActiveAndEnabled) {
                if (player != null) {
                    var colliderShockwavable =
                        colliderBody.GetComponent<ShockwavableView>();

                    if (colliderShockwavable != null) {
                        player.ShakeCamera(
                            colliderShockwavable.CameraShakeAmount,
                            colliderShockwavable.CameraShakeDuration,
                            colliderShockwavable.ShakeCameraInOneDirection);
                    }
                }

                float smashForce = UnityEngine.Random.Range(
                    smashing.MinSmashForce, smashing.MaxSmashForce);
                colliderSmashable.Smash(
                    smashForce,
                    smashing.SmashUpwardsModifier,
                    smashing.transform.position);
            }
        }
    }
}
