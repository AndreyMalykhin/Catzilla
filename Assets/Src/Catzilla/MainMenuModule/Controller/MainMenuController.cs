using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SmartLocalization;
using Catzilla.CommonModule.Model;
using Catzilla.MainMenuModule.View;

namespace Catzilla.MainMenuModule.Controller {
    public class MainMenuController {
        [Inject]
        public MainMenuView MainMenu {get; set;}

        [Inject]
        public MainScreenView MainScreen {get; set;}

        [Inject]
        public Game Game {get; set;}

        public void OnExitBtnClick() {
            Game.Exit();
        }

        public void OnStartBtnClick() {
            MainScreen.Hide();
            Game.LoadLevel();
        }
    }
}
