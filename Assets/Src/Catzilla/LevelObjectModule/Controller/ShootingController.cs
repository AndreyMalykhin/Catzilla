using UnityEngine;
using System.Collections.Generic;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.Util;
using Catzilla.LevelObjectModule.View;

namespace Catzilla.LevelObjectModule.Controller {
    public class ShootingController {
        [Inject("PlayerFieldOfViewTag")]
        public string PlayerFieldOfViewTag {get; set;}

        [Inject("EffectsLowPrioAudioChannel")]
        public int EffectsAudioChannel {get; set;}

        [Inject]
        public AudioManager AudioManager {get; set;}

        public void OnTriggerEnter(Evt evt) {
            var collider = (Collider) evt.Data;

            if (collider == null
                || !collider.CompareTag(PlayerFieldOfViewTag)) {
                return;
            }

            var shooter = (ShootingView) evt.Source;
            shooter.Target =
                collider.attachedRigidbody.GetComponent<PlayerView>().Collider;
        }

        public void OnShot(Evt evt) {
            var shooter = (ShootingView) evt.Source;
            shooter.AudioSource.pitch = Random.Range(0.9f, 1.1f);
            AudioManager.Play(
                shooter.ShotSound, shooter.AudioSource, EffectsAudioChannel);
        }
    }
}
