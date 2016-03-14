using UnityEngine;
using System.Collections.Generic;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;
using Catzilla.LevelObjectModule.View;

namespace Catzilla.LevelObjectModule.Controller {
    public class ShootingController {
        [Inject("PlayerFieldOfViewTag")]
        private string playerFieldOfViewTag;

        [Inject("EffectsLowPrioAudioChannel")]
        private int effectsAudioChannel;

        [Inject]
        private AudioManager audioManager;

        public void OnTriggerEnter(Evt evt) {
            var collider = (Collider) evt.Data;

            if (collider == null
                || !collider.CompareTag(playerFieldOfViewTag)) {
                return;
            }

            var shooter = (ShootingView) evt.Source;
            shooter.Target = collider.GetComponent<TriggerView>().Owner
                .GetComponent<PlayerView>().Collider;
        }

        public void OnShot(Evt evt) {
            var shooter = (ShootingView) evt.Source;

            if (shooter.ShotSound != null) {
                var pitch = Random.Range(0.95f, 1.05f);
                audioManager.Play(shooter.ShotSound, shooter.AudioSource,
                    effectsAudioChannel, pitch);
            }
        }
    }
}
