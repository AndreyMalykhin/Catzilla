using UnityEngine;
using System;
using System.Diagnostics;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;
using Catzilla.CommonModule.Model;
using Catzilla.PlayerModule.Model;
using Catzilla.GameOverMenuModule.View;
using Catzilla.LevelModule.View;
using Catzilla.LevelModule.Model;
using Catzilla.LevelObjectModule.View;

namespace Catzilla.LevelObjectModule.Controller {
    public class PlayerController {
        [Inject]
        private PlayerStateStorage playerStateStorage;

        [Inject]
        private LevelSettingsStorage levelSettingsStorage;

        [Inject]
        private PoolStorageView poolStorage;

        [Inject]
        private AudioManager audioManager;

        [Inject]
        private Server server;

        [Inject("PlayerHighPrioAudioChannel")]
        private int playerHighPrioAudioChannel;

        [Inject("PlayerLowPrioAudioChannel")]
        private int playerLowPrioAudioChannel;

        [Inject("PlayStopwatch")]
        private Stopwatch playStopwatch;

        [Inject]
        private Translator translator;

        [Inject("MainCamera")]
        private Camera mainCamera;

        [Inject]
        private PlayerManager playerManager;

        [Inject]
        private WorldSpacePopupManager worldSpacePopupManager;

        [Inject("SpeechWorldPopupType")]
        private int speechWorldPopupType;

        private PlayerView player;

        public void OnViewConstruct(Evt evt) {
            player = (PlayerView) evt.Source;
            playStopwatch.Reset();
            playStopwatch.Start();
            mainCamera.gameObject.SetActive(false);
        }

        public void OnViewDestroy(Evt evt) {
            if (mainCamera != null) {
                mainCamera.gameObject.SetActive(true);
            }
        }

        public void OnDeath(Evt evt) {
            playStopwatch.Stop();
            playerManager.Loose(player);
        }

        public void OnFootstep(Evt evt) {
            var shockwavable = player.GetComponent<ShockwavableView>();

            if (shockwavable != null) {
                player.ShakeCamera(
                    shockwavable.CameraShakeAmount,
                    shockwavable.CameraShakeDuration,
                    shockwavable.ShakeCameraInOneDirection);
            }

            if (player.FootstepSound != null) {
                var pitch = UnityEngine.Random.Range(0.95f, 1.05f);
                audioManager.Play(
                    player.FootstepSound,
                    player.LowPrioAudioSource,
                    playerLowPrioAudioChannel,
                    pitch);
            }
        }

        public void OnScoreChange(Evt evt) {
            LevelSettings levelSettings =
                levelSettingsStorage.Get(playerStateStorage.Get().Level);

            if (player.Score < levelSettings.CompletionScore) {
                return;
            }

            playStopwatch.Stop();
            playerManager.CompleteLevel(player);
        }

        public void OnHealthChange(Evt evt) {
            int oldHealth = (int) evt.Data;

            if (player.Health < oldHealth) {
                if (player.Health <= 0 || player.HurtSound == null) {
                    return;
                }

                var pitch = UnityEngine.Random.Range(0.95f, 1.05f);
                audioManager.Play(
                    player.HurtSound,
                    player.HighPrioAudioSource,
                    playerHighPrioAudioChannel,
                    pitch);
            } else if (player.Health > oldHealth) {
                if (player.TreatSound == null) {
                    return;
                }

                var pitch = UnityEngine.Random.Range(0.95f, 1.05f);
                audioManager.Play(player.TreatSound, player.HighPrioAudioSource,
                    playerHighPrioAudioChannel, pitch);
            }
        }

        public void OnRefuse(Evt evt) {
            WorldSpacePopupView popup =
                worldSpacePopupManager.Get(speechWorldPopupType);
            popup.Msg.text = translator.Translate("Player.Refuse");
            popup.LookAtTarget = player.Camera;
            popup.PlaceAbove(player.Collider.bounds);
            worldSpacePopupManager.Show(popup);
        }

        public void OnResurrect(Evt evt) {
            playStopwatch.Reset();
            playStopwatch.Start();
            CleanProjectiles();
        }

        private void CleanProjectiles() {
            var projectiles = (ProjectileView[]) GameObject.FindObjectsOfType(
                typeof(ProjectileView));

            for (int i = 0; i < projectiles.Length; ++i) {
                poolStorage.Return(projectiles[i].GetComponent<PoolableView>());
            }
        }
    }
}
