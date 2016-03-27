using UnityEngine;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.LevelAreaModule.View;
using Catzilla.LevelObjectModule.View;

namespace Catzilla.LevelObjectModule.Controller {
    public class ExplosiveController {
        [Inject("PlayerMeshTag")]
        private string playerMeshTag;

        public void OnTriggerEnter(Evt evt) {
            var collider = (Collider) evt.Data;

            if (collider == null) {
                return;
            }

            Rigidbody colliderBody = collider.attachedRigidbody;
            var explosive = (ExplosiveView) evt.Source;
            bool isHitPlayer = collider.CompareTag(playerMeshTag);

            if (!(isHitPlayer
                || (explosive.GetComponent<ProjectileView>() != null
                    && collider.GetComponent<EnvView>() != null)
                || (colliderBody != null
                    && colliderBody.GetComponent<LevelObjectView>() != null))) {
                return;
            }

            explosive.Explode();
        }

        public void OnExplode(Evt evt) {
            var explosionInfo = (ExplosiveView.ExplosionInfo) evt.Data;
            Collider[] hitObjects = explosionInfo.HitObjects;

            for (int i = 0; i < explosionInfo.HitObjectsCount; ++i) {
                var hitExplosive = hitObjects[i].attachedRigidbody
                    .GetComponent<ExplosiveView>();

                if (hitExplosive != null) {
                    hitExplosive.Explode();
                }
            }
        }
    }
}
