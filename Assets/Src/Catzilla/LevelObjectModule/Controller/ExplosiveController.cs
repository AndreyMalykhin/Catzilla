using UnityEngine;
using Catzilla.CommonModule.Util;
using Catzilla.LevelObjectModule.View;

namespace Catzilla.LevelObjectModule.Controller {
    public class ExplosiveController {
        public void OnExplode(Evt evt) {
            var explosionInfo = (ExplosiveView.ExplosionInfo) evt.Data;

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
    }
}
