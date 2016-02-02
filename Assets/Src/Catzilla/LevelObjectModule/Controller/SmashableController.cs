using UnityEngine;
using System.Collections;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.Util;
using Catzilla.LevelObjectModule.View;

namespace Catzilla.LevelObjectModule.Controller {
    public class SmashableController {
        [Inject("PlayerMeshTag")]
        public string PlayerTag {get; set;}

        [Inject("EffectsAudioChannel")]
        public int EffectsAudioChannel {get; set;}

        [Inject]
        public AudioManager AudioManager {get; set;}

        public void OnTriggerEnter(Evt evt) {
            var collider = (Collider) evt.Data;

            if (collider == null || !collider.CompareTag(PlayerTag)) {
                return;
            }

            var smashable = (SmashableView) evt.Source;
            SmashedView smashed =
                smashable.Smash(collider.attachedRigidbody.position);
            smashed.AudioSource.pitch = Random.Range(0.9f, 1.1f);
            AudioManager.Play(smashed.SmashSound, smashed.AudioSource,
                EffectsAudioChannel);
        }
    }
}
