using UnityEngine;
using Catzilla.CommonModule.Model;
using Catzilla.LeaderboardModule.Model;
using Catzilla.MainMenuModule.View;

namespace Catzilla.MainMenuModule.Controller {
    public class MainMenuController {
        [Inject]
        public MainMenuView MainMenu {get; set;}

        [Inject]
        public MainScreenView MainScreen {get; set;}

        [Inject]
        public Game Game {get; set;}

        [Inject]
        public LeaderboardManager LeaderboardManager {get; set;}

        public void OnServerDispose() {
            MainMenu.LeaderboardBtn.interactable = false;
        }

        public void OnExitBtnClick() {
            Game.Exit();
        }

        public void OnStartBtnClick() {
            MainScreen.Hide();
            Game.LoadLevel();
        }

        public void OnLeaderboardBtnClick() {
            LeaderboardManager.Show();
        }
    }
}
