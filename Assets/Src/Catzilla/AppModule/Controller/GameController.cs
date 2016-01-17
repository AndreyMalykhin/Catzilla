using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using SmartLocalization;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.View;
using Catzilla.PlayerModule.Model;
using Catzilla.LevelModule.Controller;
using Catzilla.LevelModule.View;
using Catzilla.MainMenuModule.View;
using Catzilla.GameOverMenuModule.View;

namespace Catzilla.AppModule.Controller {
    public class GameController {
        [Inject]
        public MainScreenView MainScreen {get; set;}

        [Inject]
        public GameOverScreenView GameOverScreen {get; set;}

        [Inject("LevelScene")]
        public string LevelScene {get; set;}

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
