using UnityEngine;
using Catzilla.CommonModule.Util;
using Catzilla.LevelObjectModule.View;

namespace Catzilla.LevelObjectModule.Controller {
    public class ExplosiveController {
        private PlayerView player;

        public void OnPlayerConstruct(Evt evt) {
            player = (PlayerView) evt.Source;
        }

        public void OnExplode(Evt evt) {
            var explosionInfo = (ExplosiveView.ExplosionInfo) evt.Data;
            AffectHitObjects(explosionInfo);
            AffectPlayer((ExplosiveView) evt.Source);
        }

        private void AffectHitObjects(
            ExplosiveView.ExplosionInfo explosionInfo) {
            for (int i = 0; i < explosionInfo.HitObjectsCount; ++i) {
                Rigidbody hitBody =
                    explosionInfo.HitObjects[i].attachedRigidbody;
                var hitExplosive = hitBody.GetComponent<ExplosiveView>();

                if (hitExplosive != null) {
                    if (hitExplosive.isActiveAndEnabled) {
                        hitExplosive.Explode();
                    }
                } else {
                    var hitSmashable = hitBody.GetComponent<SmashableView>();

                    if (hitSmashable != null
                        && hitSmashable.isActiveAndEnabled) {
                        float smashUpwardsModifier = 0f;
                        hitSmashable.Smash(
                            explosionInfo.Force,
                            smashUpwardsModifier,
                            explosionInfo.Position);
                    }
                }
            }
        }

        private void AffectPlayer(ExplosiveView explosive) {
            var shockwavable = explosive.GetComponent<ShockwavableView>();

            if (shockwavable != null) {
                player.ShakeCamera(
                    shockwavable.CameraShakeAmount,
                    shockwavable.CameraShakeDuration,
                    shockwavable.ShakeCameraInOneDirection);
            }
        }
    }
}
