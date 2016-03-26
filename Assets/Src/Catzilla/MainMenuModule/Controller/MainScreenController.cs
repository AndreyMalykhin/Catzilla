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
            string subject = WWW.EscapeURL("Feedback");
            string url =
                string.Format("mailto:{0}?subject={1}", FeedbackEmail, subject);
            Application.OpenURL(url);
        }

        public void OnLoginBtnClick() {
            AuthManager.OnLoginSuccess += OnLogin;
            AuthManager.Login();
        }

        public void OnServerDispose(Evt evt) {
            MainScreen.LeaderboardBtn.interactable = false;
            MainScreen.AchievementsBtn.interactable = false;
            MainScreen.LoginBtn.interactable = false;
        }

        public void OnExitBtnClick() {
            Application.Quit();
        }

        public void OnStartBtnClick() {
            MainScreen.GetComponent<ShowableView>().Hide();
            Game.LoadLevel();
        }

        public void OnLeaderboardBtnClick() {
            AuthManager.OnLoginSuccess += OnLoginForLeaderboard;
            AuthManager.Login();
        }

        public void OnAchievementsBtnClick() {
            AuthManager.OnLoginSuccess += OnLoginForAchievements;
            AuthManager.Login();
        }

        public void OnReplaysBtnClick() {
            Everyplay.ShowWithPath("/feed/game");
        }

        private void OnLoginForLeaderboard(AuthManager authManager) {
            authManager.OnLoginSuccess += OnLoginForLeaderboard;
            OnLogin(authManager);
            Social.ShowLeaderboardUI();
        }

        private void OnLoginForAchievements(AuthManager authManager) {
            authManager.OnLoginSuccess += OnLoginForAchievements;
            OnLogin(authManager);
            Social.ShowAchievementsUI();
        }

        private void OnLogin(AuthManager authManager) {
            authManager.OnLoginSuccess -= OnLogin;
            MainScreen.LoginBtn.gameObject.SetActive(false);
        }
    }
}
