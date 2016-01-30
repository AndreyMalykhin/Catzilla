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

        public void OnDeath(IEvent evt) {
            var player = (PlayerView) evt.data;
            PlayerState playerState = PlayerStateStorage.Get();
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

        public void OnScoreChange(IEvent evt) {
            var player = (PlayerView) evt.data;
            LevelSettings levelSettings = LevelSettingsStorage.Get(level.Index);

            if (player.Score < levelSettings.CompletionScore) {
                return;
            }

            CompleteLevel(player);
        }

        public void OnLevelConstruct(IEvent evt) {
            level = (LevelView) evt.data;
        }

        public void OnResurrect(IEvent evt) {
            CleanProjectiles();
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
            player.IsHealthFreezed = true;
            player.IsScoreFreezed = true;
            PlayerState playerState = PlayerStateStorage.Get();
            ++playerState.Level;
            ++playerState.AvailableResurrectionsCount;
            playerState.ScoreRecord = player.Score;
            PlayerStateStorage.Save(playerState);

            if (!Server.IsDisposed) {
                PlayerStateStorage.Sync(Server);
            }

            LevelCompleteScreen.Show();
        }
    }
}
