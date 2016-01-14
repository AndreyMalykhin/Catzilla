using UnityEngine;
using System.Collections;
using Catzilla.PlayerModule.Model;
using Catzilla.LevelModule.Controller;
using Catzilla.MainMenuModule.View;
using Catzilla.GameOverMenuModule.View;

namespace Catzilla.AppModule.Controller {
    public class GameController {
        [Inject]
        public PlayerSettingsStorage PlayerSettingsStorage {get; set;}

        [Inject]
        public LevelController LevelController {get; set;}

        [Inject]
        public MainScreenView MainScreen {get; set;}

        [Inject]
        public GameOverScreenView GameOverScreen {get; set;}

        [Inject("MainCamera")]
        public Camera MainCamera {get; set;}

        public void OnStart() {
            PlayerSettings playerSettings = PlayerSettingsStorage.GetCurrent();
            MainScreen.Hide();
            MainCamera.gameObject.SetActive(false);
            LevelController.Load(playerSettings.Level);
        }

        public void OnRestart() {
            PlayerSettings playerSettings = PlayerSettingsStorage.GetCurrent();
            LevelController.Load(playerSettings.Level);
            GameOverScreen.Hide();
        }

        public void OnPlayerDeath() {
            GameOverScreen.Show();
        }

        public void OnPause() {
            Time.timeScale = 0f;
            AudioListener.pause = true;
        }

        public void OnResume() {
            Time.timeScale = 1f;
            AudioListener.pause = false;
        }
    }
}
