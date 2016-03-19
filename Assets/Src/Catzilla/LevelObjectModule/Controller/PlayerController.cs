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
        public GameOverScreenView GameOverScreen {get; set;}

        [Inject]
        public PlayerStateStorage PlayerStateStorage {get; set;}

        [Inject]
        public LevelCompleteScreenView LevelCompleteScreen {get; set;}

        [Inject]
        public LevelSettingsStorage LevelSettingsStorage {get; set;}

        [Inject]
        public PoolStorageView PoolStorage {get; set;}

        [Inject]
        public AudioManager AudioManager {get; set;}

        [Inject]
        public Server Server {get; set;}

        [Inject]
        public Game Game {get; set;}

        [Inject("PlayerHighPrioAudioChannel")]
        public int PlayerHighPrioAudioChannel {get; set;}

        [Inject("PlayerLowPrioAudioChannel")]
        public int PlayerLowPrioAudioChannel {get; set;}

        [Inject("PlayStopwatch")]
        public Stopwatch PlayStopwatch {get; set;}

        [Inject]
        public GiftManager GiftManager {get; set;}

        [Inject]
        public ScreenSpacePopupManagerView PopupManager {get; set;}

        [Inject]
        public Translator Translator {get; set;}

        [Inject("CommonPopupType")]
        public int CommonPopupType {get; set;}

        [Inject]
        public RewardManager RewardManager {get; set;}

        [Inject("MainCamera")]
        public Camera MainCamera {get; set;}

        [Inject]
        public PlayerManager PlayerManager {get; set;}

        private PlayerView player;

        public void OnViewConstruct(Evt evt) {
            player = (PlayerView) evt.Source;
            PlayStopwatch.Reset();
            PlayStopwatch.Start();
            MainCamera.gameObject.SetActive(false);
        }

        public void OnViewDestroy(Evt evt) {
            if (MainCamera != null) {
                MainCamera.gameObject.SetActive(true);
            }
        }

        public void OnDeath(Evt evt) {
            PlayerState playerState = PlayerStateStorage.Get();
            AnalyticsUtils.AddCategorizedEventParam("Level", playerState.Level);
            AnalyticsUtils.LogEvent("Player.Death");
            PlayStopwatch.Stop();
            playerState.ScoreRecord = player.Score;
            playerState.PlayTime += PlayStopwatch.Elapsed;
            PlayerStateStorage.Save(playerState);

            if (Server.IsLoggedIn) {
                PlayerStateStorage.Sync(Server);
            }

            GameOverScreen.GetComponent<ShowableView>().Show();

            if (player.DeathSound != null) {
                AudioManager.Play(player.DeathSound, player.HighPrioAudioSource,
                    PlayerHighPrioAudioChannel);
            }
        }

        public void OnFootstep(Evt evt) {
            if (player.FootstepSound == null) {
                return;
            }

            var pitch = UnityEngine.Random.Range(0.95f, 1.05f);
            AudioManager.Play(
                player.FootstepSound,
                player.LowPrioAudioSource,
                PlayerLowPrioAudioChannel,
                pitch);
        }

        public void OnScoreChange(Evt evt) {
            LevelSettings levelSettings =
                LevelSettingsStorage.Get(PlayerStateStorage.Get().Level);

            if (player.Score < levelSettings.CompletionScore) {
                return;
            }

            PlayStopwatch.Stop();
            PlayerManager.CompleteLevel(player);
        }

        public void OnHealthChange(Evt evt) {
            int oldHealth = (int) evt.Data;

            if (player.Health < oldHealth) {
                if (player.Health <= 0 || player.HurtSound == null) {
                    return;
                }

                var pitch = UnityEngine.Random.Range(0.95f, 1.05f);
                AudioManager.Play(
                    player.HurtSound,
                    player.HighPrioAudioSource,
                    PlayerHighPrioAudioChannel,
                    pitch);
            } else if (player.Health > oldHealth) {
                if (player.TreatSound == null) {
                    return;
                }

                var pitch = UnityEngine.Random.Range(0.95f, 1.05f);
                AudioManager.Play(player.TreatSound, player.HighPrioAudioSource,
                    PlayerHighPrioAudioChannel, pitch);
            }
        }

        public void OnResurrect(Evt evt) {
            PlayStopwatch.Reset();
            PlayStopwatch.Start();
            AnalyticsUtils.AddCategorizedEventParam(
                "Level", PlayerStateStorage.Get().Level);
            AnalyticsUtils.LogEvent("Player.Resurrection");
            CleanProjectiles();
        }

        private void CleanProjectiles() {
            var projectiles = (ProjectileView[]) GameObject.FindObjectsOfType(
                typeof(ProjectileView));

            for (int i = 0; i < projectiles.Length; ++i) {
                PoolStorage.Return(projectiles[i].GetComponent<PoolableView>());
            }
        }
    }
}
