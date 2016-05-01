using UnityEngine;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.PlayerModule.Model;
using Catzilla.LevelModule.Model;
using Catzilla.LevelObjectModule.View;

namespace Catzilla.LevelObjectModule.Controller {
    public class TakeableController {
        [Inject("PlayerMeshTag")]
        private string playerMeshTag;

        [Inject]
        private AudioManager audioManager;

        [Inject("PlayerMidPrioAudioChannel")]
        private int playerMidPrioAudioChannel;

        public void OnTriggerEnter(Evt evt) {
            var collider = (Collider) evt.Data;

            if (collider == null || !collider.CompareTag(playerMeshTag)) {
                return;
            }

            var takeable = (TakeableView) evt.Source;

            if (takeable.TakeSound != null) {
                var player =
                    collider.attachedRigidbody.GetComponent<PlayerView>();
                var pitch = UnityEngine.Random.Range(0.95f, 1.05f);
                audioManager.Play(
                    takeable.TakeSound,
                    player.MidPrioAudioSource,
                    playerMidPrioAudioChannel,
                    pitch);
            }

            takeable.Take();
        }
    }
}
