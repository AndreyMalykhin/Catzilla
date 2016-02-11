using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.LevelObjectModule.View;

namespace Catzilla.LevelObjectModule.Controller {
    public class SmashableController {
        [Inject("PlayerMeshTag")]
        public string PlayerTag {get; set;}

        [Inject("EffectsHighPrioAudioChannel")]
        public int EffectsAudioChannel {get; set;}

        [Inject]
        public AudioManager AudioManager {get; set;}

        public void OnTriggerEnter(Evt evt) {
            var collider = (Collider) evt.Data;

            if (collider == null || !collider.CompareTag(PlayerTag)) {
                return;
            }

            var smashable = (SmashableView) evt.Source;
            var player = collider.attachedRigidbody.GetComponent<PlayerView>();
            SmashedView smashed = smashable.Smash(
                Random.Range(player.MinSmashForce, player.MaxSmashForce),
                player.SmashUpwardsModifier,
                collider.attachedRigidbody.position);

            if (smashed.SmashSound != null) {
                var pitch = Random.Range(0.9f, 1.1f);
                AudioManager.Play(smashed.SmashSound, smashed.AudioSource,
                    EffectsAudioChannel, pitch);
            }
        }
    }
}
