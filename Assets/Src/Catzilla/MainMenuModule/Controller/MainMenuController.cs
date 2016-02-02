using UnityEngine;
using Catzilla.CommonModule.Model;
using Catzilla.CommonModule.Util;
using Catzilla.LeaderboardModule.Model;
using Catzilla.PlayerModule.Model;
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

        [Inject]
        public PlayerStateStorage PlayerStateStorage {get; set;}

        public void OnServerDispose(Evt evt) {
            MainMenu.LeaderboardBtn.interactable = false;
        }

        public void OnExitBtnClick(Evt evt) {
            AnalyticsUtils.LogEvent("Game.Exit");
            Game.Exit();
        }

        public void OnStartBtnClick(Evt evt) {
            AnalyticsUtils.LogEvent("Game.Start");
            MainScreen.Hide();
            Game.LoadLevel();
        }

        public void OnLeaderboardBtnClick(Evt evt) {
            AnalyticsUtils.AddEventParam("Origin", "MainMenu");
            AnalyticsUtils.AddCategorizedEventParam(
                "Level", PlayerStateStorage.Get().Level);
            AnalyticsUtils.LogEvent("Leaderboard.View");
            LeaderboardManager.Show();
        }
    }
}
