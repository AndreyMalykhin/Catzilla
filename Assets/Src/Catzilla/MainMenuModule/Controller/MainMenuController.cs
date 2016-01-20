using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SmartLocalization;
using Catzilla.CommonModule.Model;
using Catzilla.MainMenuModule.View;

namespace Catzilla.MainMenuModule.Controller {
    public class MainMenuController {
        [Inject]
        public LanguageManager Translator {get; set;}

        [Inject]
        public MainMenuView MainMenu {get; set;}

        [Inject]
        public MainScreenView MainScreen {get; set;}

        [Inject]
        public Game Game {get; set;}

        [PostConstruct]
        public void OnConstruct() {
            MainMenu.StartText.text = Translator.GetTextValue("MainMenu.Start");
            MainMenu.ExitText.text = Translator.GetTextValue("MainMenu.Exit");
        }

        public void OnExitBtnClick() {
            Game.Exit();
        }

        public void OnStartBtnClick() {
            MainScreen.Hide();
            Game.LoadLevel();
        }
    }
}
