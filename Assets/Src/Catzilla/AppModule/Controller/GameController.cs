using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using SmartLocalization;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.View;
using Catzilla.PlayerModule.Model;
using Catzilla.LevelModule.Controller;
using Catzilla.LevelModule.View;
using Catzilla.LevelObjectModule.View;
using Catzilla.MainMenuModule.View;
using Catzilla.GameOverMenuModule.View;

namespace Catzilla.AppModule.Controller {
    public class GameController {
        [Inject]
        public MainScreenView MainScreen {get; set;}

        [Inject]
        public GameOverScreenView GameOverScreen {get; set;}

        [Inject]
        public PlayerSettingsStorage PlayerSettingsStorage {get; set;}

        [Inject]
        public LevelCompleteScreenView LevelCompleteScreen {get; set;}

        [Inject("LevelScene")]
        public string LevelScene {get; set;}

        private LevelView level;

        public void OnStartBtnClick() {
            MainScreen.Hide();
            LoadLevel();
        }

        public void OnRestartBtnClick() {
            GameOverScreen.Hide(() => {
                LoadLevel();
            });
        }

        public void OnPlayerDeath() {
            GameOverScreen.Show(() => {
                Pause();
            });
        }

        public void OnPlayerScoreChange(IEvent evt) {
            var player = (PlayerView) evt.data;

            if (player.Score < level.CompletionScore) {
                return;
            }

            CompleteLevel(player);
        }

        public void OnLevelReady(IEvent evt) {
            level = (LevelView) evt.data;
        }

        public void OnLevelCompleteScreenHide() {
            LoadLevel();
        }

        private void CompleteLevel(PlayerView player) {
            player.IsHealthFreezed = true;
            player.IsScoreFreezed = true;
            PlayerSettingsStorage.GetCurrent((playerSettings) => {
                ++playerSettings.Level;
                playerSettings.MaxScore = player.Score;
                PlayerSettingsStorage.Update(playerSettings, () => {
                    LevelCompleteScreen.Show();
                });
            });
        }

        private void LoadLevel() {
            SceneManager.LoadScene(LevelScene);
            Resume();
        }

        private void Pause() {
            Time.timeScale = 0f;
            AudioListener.pause = true;
        }

        private void Resume() {
            Time.timeScale = 1f;
            AudioListener.pause = false;
        }
    }
}
