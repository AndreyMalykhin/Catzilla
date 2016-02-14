using UnityEngine;
using System;
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

        [Inject("ProjectileTag")]
        public string ProjectileTag {get; set;}

        [Inject("PlayerHighPrioAudioChannel")]
        public int PlayerHighPrioAudioChannel {get; set;}

        [Inject("PlayerLowPrioAudioChannel")]
        public int PlayerLowPrioAudioChannel {get; set;}

        public void OnDeath(Evt evt) {
            var player = (PlayerView) evt.Source;
            PlayerState playerState = PlayerStateStorage.Get();
            AnalyticsUtils.AddCategorizedEventParam("Level", playerState.Level);
            AnalyticsUtils.LogEvent("Player.Death");
            playerState.ScoreRecord =
                Mathf.Max(playerState.ScoreRecord, player.Score);
            PlayerStateStorage.Save(playerState);

            if (Server.IsLoggedIn) {
                PlayerStateStorage.Sync(Server);
            }

            GameOverScreen.Show(OnGameOverScreenShow);

            if (player.DeathSound != null) {
                AudioManager.Play(player.DeathSound, player.HighPrioAudioSource,
                    PlayerHighPrioAudioChannel);
            }
        }

        public void OnFootstep(Evt evt) {
            var player = (PlayerView) evt.Source;

            if (player.FootstepSound != null) {
                var pitch = UnityEngine.Random.Range(0.8f, 1.2f);
                AudioManager.Play(
                    player.FootstepSound,
                    player.LowPrioAudioSource,
                    PlayerLowPrioAudioChannel, pitch);
            }
        }

        public void OnScoreChange(Evt evt) {
            var player = (PlayerView) evt.Source;
            LevelSettings levelSettings =
                LevelSettingsStorage.Get(PlayerStateStorage.Get().Level);

            if (player.Score < levelSettings.CompletionScore) {
                return;
            }

            CompleteLevel(player);
        }

        public void OnResurrect(Evt evt) {
            CleanProjectiles();
            AnalyticsUtils.AddCategorizedEventParam(
                "Level", PlayerStateStorage.Get().Level);
            AnalyticsUtils.LogEvent("Player.Resurrection");
        }

        private void OnGameOverScreenShow() {
            Game.Pause();
        }

        private void CleanProjectiles() {
            GameObject[] projectiles =
                GameObject.FindGameObjectsWithTag(ProjectileTag);

            for (int i = 0; i < projectiles.Length; ++i) {
                PoolStorage.Return(projectiles[i].GetComponent<PoolableView>());
            }
        }

        private void CompleteLevel(PlayerView player) {
            DebugUtils.Log(
                "PlayerController.CompleteLevel(); {0}", DateTime.Now);
            player.IsHealthFreezed = true;
            player.IsScoreFreezed = true;
            PlayerState playerState = PlayerStateStorage.Get();
            AnalyticsUtils.AddCategorizedEventParam("Level", playerState.Level);
            AnalyticsUtils.LogEvent("Level.Completion");
            ++playerState.Level;
            ++playerState.AvailableResurrectionsCount;
            playerState.ScoreRecord =
                Mathf.Max(playerState.ScoreRecord, player.Score);
            PlayerStateStorage.Save(playerState);

            if (Server.IsLoggedIn) {
                PlayerStateStorage.Sync(Server);
            }

            LevelCompleteScreen.OnHide = OnLevelCompleteScreenHide;
            LevelCompleteScreen.Show();
        }

        private void OnLevelCompleteScreenHide() {
            Game.LoadLevel();
        }
    }
}
