using UnityEngine;
using System.Collections;
using strange.extensions.dispatcher.eventdispatcher.api;
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

        [Inject("PlayerAudioChannel")]
        public int PlayerAudioChannel {get; set;}

        private LevelView level;

        public void OnDeath(Evt evt) {
            var player = (PlayerView) evt.Source;
            PlayerState playerState = PlayerStateStorage.Get();
            AnalyticsUtils.AddCategorizedEventParam("Level", playerState.Level);
            AnalyticsUtils.LogEvent("Player.Death");
            playerState.ScoreRecord =
                Mathf.Max(playerState.ScoreRecord, player.Score);
            PlayerStateStorage.Save(playerState);

            if (!Server.IsDisposed) {
                PlayerStateStorage.Sync(Server);
            }

            GameOverScreen.Show(OnGameOverScreenShow);
            AudioManager.Play(
                player.DeathSound, player.AudioSource, PlayerAudioChannel);
        }

        public void OnScoreChange(Evt evt) {
            var player = (PlayerView) evt.Source;
            LevelSettings levelSettings = LevelSettingsStorage.Get(level.Index);

            if (player.Score < levelSettings.CompletionScore) {
                return;
            }

            CompleteLevel(player);
        }

        public void OnLevelConstruct(Evt evt) {
            level = (LevelView) evt.Source;
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

        // TODO move Save()
        private void CompleteLevel(PlayerView player) {
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

            if (!Server.IsDisposed) {
                PlayerStateStorage.Sync(Server);
            }

            LevelCompleteScreen.Show();
        }
    }
}
