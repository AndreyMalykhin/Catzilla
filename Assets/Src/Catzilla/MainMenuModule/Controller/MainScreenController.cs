using UnityEngine;
using Zenject;
using Catzilla.CommonModule.Model;
using Catzilla.CommonModule.Util;
using Catzilla.PlayerModule.Model;
using Catzilla.MainMenuModule.View;

namespace Catzilla.MainMenuModule.Controller {
    public class MainScreenController {
        [Inject]
        public MainScreenView MainScreen {get; set;}

        [Inject]
        public Game Game {get; set;}

        [Inject]
        public PlayerStateStorage PlayerStateStorage {get; set;}

        [Inject]
        public AuthManager AuthManager {get; set;}

        [Inject]
        public Server Server {get; set;}

        [PostInject]
        public void OnConstruct() {
            MainScreen.LoginBtn.gameObject.SetActive(!Server.IsLoggedIn);
        }

        public void OnLoginBtnClick() {
            AuthManager.Login(OnLogin);
        }

        public void OnServerDispose(Evt evt) {
            MainScreen.Menu.LeaderboardBtn.interactable = false;
            MainScreen.Menu.AchievementsBtn.interactable = false;
            MainScreen.LoginBtn.interactable = false;
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
            AnalyticsUtils.AddCategorizedEventParam(
                "Level", PlayerStateStorage.Get().Level);
            AnalyticsUtils.LogEvent("Leaderboard.View");
            AuthManager.Login(OnLoginForLeaderboard);
        }

        public void OnAchievementsBtnClick(Evt evt) {
            AnalyticsUtils.AddCategorizedEventParam(
                "Level", PlayerStateStorage.Get().Level);
            AnalyticsUtils.LogEvent("Achievements.View");
            AuthManager.Login(OnLoginForAchievements);
        }

        private void OnLoginForLeaderboard() {
            OnLogin();
            Social.ShowLeaderboardUI();
        }

        private void OnLoginForAchievements() {
            OnLogin();
            Social.ShowAchievementsUI();
        }

        private void OnLogin() {
            MainScreen.LoginBtn.gameObject.SetActive(false);
        }
    }
}
