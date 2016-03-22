using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;
using Catzilla.LevelAreaModule.View;
using Catzilla.LevelObjectModule.View;

namespace Catzilla.LevelObjectModule.Controller {
    public class ProjectileController {
        [Inject]
        private PoolStorageView poolStorage;

        [Inject("PlayerMeshTag")]
        private string playerMeshTag;

        public void OnTriggerEnter(Evt evt) {
            var collider = (Collider) evt.Data;

            if (collider == null) {
                return;
            }

            Rigidbody colliderBody = collider.attachedRigidbody;
            bool isHitPlayer = collider.CompareTag(playerMeshTag);

            if (!isHitPlayer
                && collider.GetComponent<EnvView>() == null
                && colliderBody != null
                && colliderBody.GetComponent<LevelObjectView>() == null) {
                return;
            }

            var projectile = (ProjectileView) evt.Source;
            PlayerView player = null;

            if (isHitPlayer) {
                player = colliderBody.GetComponent<PlayerView>();
                var damaging = projectile.GetComponent<DamagingView>();

                if (damaging != null) {
                    player.Health -= damaging.Damage;
                }
            }

            var explosive = projectile.GetComponent<ExplosiveView>();

            if (explosive != null) {
                if (explosive.isActiveAndEnabled) {
                    explosive.Explode();
                }
            } else if (isHitPlayer) {
                var shockwavable = projectile.GetComponent<ShockwavableView>();

                if (shockwavable != null) {
                    player.ShakeCamera(
                        shockwavable.CameraShakeAmount,
                        shockwavable.CameraShakeDuration,
                        shockwavable.ShakeCameraInOneDirection);
                }

                poolStorage.Return(projectile.Poolable);
            }
        }
    }
}
