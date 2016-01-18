using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SmartLocalization;
using Catzilla.AppModule.Model;
using Catzilla.GameOverMenuModule.View;

namespace Catzilla.GameOverMenuModule.Controller {
    public class GameOverMenuController {
        [Inject]
        public LanguageManager Translator {get; set;}

        [Inject]
        public GameOverMenuView GameOverMenu {get; set;}

        [Inject]
        public GameOverScreenView GameOverScreen {get; set;}

        [Inject]
        public Game Game {get; set;}

        [PostConstruct]
        public void OnReady() {
            GameOverMenu.RestartText.text =
                Translator.GetTextValue("GameOverMenu.Restart");
            GameOverMenu.ExitText.text =
                Translator.GetTextValue("GameOverMenu.Exit");
        }

        public void OnExitBtnClick() {
            Game.Exit();
        }

        public void OnRestartBtnClick() {
            GameOverScreen.Hide(() => {
                Game.LoadLevel();
                Game.Resume();
            });
        }
    }
}
