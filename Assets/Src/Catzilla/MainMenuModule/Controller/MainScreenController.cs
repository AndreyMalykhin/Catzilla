using UnityEngine;
using Zenject;
using Catzilla.CommonModule.Model;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;
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

        [Inject("FeedbackEmail")]
        public string FeedbackEmail {get; set;}

        [PostInject]
        public void OnConstruct() {
            MainScreen.LoginBtn.gameObject.SetActive(!Server.IsLoggedIn);
        }

        public void OnFeedbackBtnClick() {
            AnalyticsUtils.LogEvent("Feedback.Leave");
            string subject = WWW.EscapeURL("Feedback");
            string url =
                string.Format("mailto:{0}?subject={1}", FeedbackEmail, subject);
            Application.OpenURL(url);
        }

        public void OnLoginBtnClick() {
            AnalyticsUtils.LogEvent("Player.Login");
            AuthManager.Login(OnLogin);
        }

        public void OnServerDispose(Evt evt) {
            MainScreen.Menu.LeaderboardBtn.interactable = false;
            MainScreen.Menu.AchievementsBtn.interactable = false;
            MainScreen.LoginBtn.interactable = false;
        }

        public void OnExitBtnClick() {
            Game.Exit();
        }

        public void OnStartBtnClick() {
            AnalyticsUtils.LogEvent("Game.Start");
            MainScreen.GetComponent<ShowableView>().Hide();
            Game.LoadLevel();
        }

        public void OnLeaderboardBtnClick() {
            AnalyticsUtils.AddCategorizedEventParam(
                "Level", PlayerStateStorage.Get().Level);
            AnalyticsUtils.LogEvent("Leaderboard.View");
            AuthManager.Login(OnLoginForLeaderboard);
        }

        public void OnAchievementsBtnClick() {
            AnalyticsUtils.AddCategorizedEventParam(
                "Level", PlayerStateStorage.Get().Level);
            AnalyticsUtils.LogEvent("Achievements.View");
            AuthManager.Login(OnLoginForAchievements);
        }

        public void OnReplaysBtnClick() {
            Everyplay.ShowWithPath("/feed/game");
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
