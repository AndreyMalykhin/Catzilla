using UnityEngine;
using System.Collections;
using System.Text;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;
using Catzilla.PlayerModule.Model;
using Catzilla.LevelObjectModule.View;

namespace Catzilla.LevelObjectModule.Controller {
    public class SmashableController {
        [Inject("EffectsHighPrioAudioChannel")]
        private int effectsAudioChannel;

        [Inject]
        private AudioManager audioManager;

        public void OnSmash(Evt evt) {
            var smashed = (SmashedView) evt.Data;

            if (smashed.SmashSound != null) {
                var pitch = UnityEngine.Random.Range(0.95f, 1.05f);
                audioManager.Play(smashed.SmashSound, smashed.AudioSource,
                    effectsAudioChannel, pitch);
            }
        }

        public void OnTriggerEnter(Evt evt) {
            var collider = (Collider) evt.Data;
            Rigidbody colliderBody = collider.attachedRigidbody;

            if (collider == null || colliderBody == null) {
                return;
            }

            var colliderSmashing = colliderBody.GetComponent<SmashingView>();
            var smashable = (SmashableView) evt.Source;

            if (colliderSmashing == null
                || smashable.GetComponent<ExplosiveView>() != null) {
                return;
            }

            float smashForce = UnityEngine.Random.Range(
                colliderSmashing.MinSmashForce, colliderSmashing.MaxSmashForce);
            smashable.Smash(
                smashForce,
                colliderSmashing.SmashUpwardsModifier,
                colliderSmashing.transform.position);
        }

        public void OnExplosion(Evt evt) {
            var explosive = (ExplosiveView) evt.Source;
            var explosionInfo = (ExplosiveView.ExplosionInfo) evt.Data;
            var smashable = explosive.GetComponent<SmashableView>();

            if (smashable != null) {
                smashable.Smash(
                    explosionInfo.Force,
                    explosive.ExplosionUpwardsModifier,
                    explosionInfo.Position);
            }

            Collider[] hitObjects = explosionInfo.HitObjects;

            for (int i = 0; i < explosionInfo.HitObjectsCount; ++i) {
                var hitSmashable = hitObjects[i].attachedRigidbody
                    .GetComponent<SmashableView>();

                if (hitSmashable != null
                    && hitSmashable.GetComponent<ExplosiveView>() == null) {
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
