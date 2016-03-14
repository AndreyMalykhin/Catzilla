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
        private PlayerManager playerManager;

        [Inject]
        private AudioManager audioManager;

        [Inject("PlayerHighPrioAudioChannel")]
        private int playerHighPrioAudioChannel;

        public void OnTriggerEnter(Evt evt) {
            var collider = (Collider) evt.Data;

            if (collider == null || !collider.CompareTag(playerMeshTag)) {
                return;
            }

            var takeable = (TakeableView) evt.Source;
            var scoreBonus = takeable.GetComponent<ScoreBonusView>();
            var player =
                collider.attachedRigidbody.GetComponent<PlayerView>();

            if (scoreBonus != null) {
                ++player.ScoreBonusesTaken;
            }

            var scoreable = takeable.GetComponent<ScoreableView>();

            if (scoreable != null) {
                playerManager.AddScore(player, scoreable);
            }

            var treating = takeable.GetComponent<TreatingView>();

            if (treating != null) {
                player.Health += treating.AddHealth;
            }

            if (takeable.TakeSound != null) {
                var pitch = UnityEngine.Random.Range(0.95f, 1.05f);
                audioManager.Play(
                    takeable.TakeSound,
                    player.HighPrioAudioSource,
                    playerHighPrioAudioChannel,
                    pitch);
            }

            takeable.Take();
        }
    }
}
