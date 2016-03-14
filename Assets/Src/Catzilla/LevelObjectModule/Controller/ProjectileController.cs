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

            bool isHitPlayer = collider.CompareTag(playerMeshTag);

            if (!isHitPlayer
                && collider.GetComponent<EnvView>() == null
                && collider.attachedRigidbody != null
                && collider.attachedRigidbody.GetComponent<LevelObjectView>() == null) {
                return;
            }

            var projectile = (ProjectileView) evt.Source;
            var explosive = projectile.GetComponent<ExplosiveView>();

            if (explosive != null) {
                if (explosive.isActiveAndEnabled) {
                    explosive.Explode();
                }
            } else if (isHitPlayer) {
                poolStorage.Return(projectile.Poolable);
            }
        }
    }
}
