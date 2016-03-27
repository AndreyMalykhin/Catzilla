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

            if (!(collider.CompareTag(playerMeshTag)
                || collider.GetComponent<EnvView>() != null
                || (colliderBody != null
                    && colliderBody.GetComponent<LevelObjectView>() != null
                    && colliderBody.GetComponent<ExplosiveView>() != null))) {
                return;
            }

            var projectile = (ProjectileView) evt.Source;
            projectile.Hit();
        }
    }
}
