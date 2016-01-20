using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SmartLocalization;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.Model;
using Catzilla.CommonModule.Util;
using Catzilla.LevelObjectModule.View;
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

        [Inject]
        public Ad Ad {get; set;}

        private PlayerView player;

        [PostConstruct]
        public void OnConstruct() {
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

        public void OnPlayerConstruct(IEvent evt) {
            player = (PlayerView) evt.data;
        }

        public void OnResurrectBtnClick() {
            Ad.Show(() => {
                GameOverScreen.Hide(() => {
                    player.Resurrect();
                    Game.Resume();
                });
            });
        }
    }
}
