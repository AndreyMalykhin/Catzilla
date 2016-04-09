using UnityEngine;
using Zenject;
using Catzilla.CommonModule.Model;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;
using Catzilla.PlayerModule.Model;
using Catzilla.SkillModule.View;
using Catzilla.SkillModule.Model;
using Catzilla.MainMenuModule.View;

namespace Catzilla.MainMenuModule.Controller {
    public class MainScreenController {
        [Inject]
        private MainScreenView mainScreen;

        [Inject]
        private Game game;

        [Inject]
        private AuthManager authManager;

        [Inject]
        private Server server;

        [Inject("FeedbackEmail")]
        private string feedbackEmail;

        [Inject]
        private SkillManager skillManager;

        [PostInject]
        public void OnConstruct() {
            mainScreen.LoginBtn.gameObject.SetActive(!server.IsLoggedIn);
        }

        public void OnSkillsBtnClick() {
            skillManager.ShowScreen();
        }

        public void OnFeedbackBtnClick() {
            string subject = WWW.EscapeURL("Feedback");
            string url =
                string.Format("mailto:{0}?subject={1}", feedbackEmail, subject);
            Application.OpenURL(url);
        }

        public void OnLoginBtnClick() {
            authManager.OnLoginSuccess += OnLogin;
            authManager.Login();
        }

        public void OnServerDispose(Evt evt) {
            mainScreen.LeaderboardBtn.interactable = false;
            mainScreen.AchievementsBtn.interactable = false;
            mainScreen.LoginBtn.interactable = false;
        }

        public void OnExitBtnClick() {
            Application.Quit();
        }

        public void OnStartBtnClick() {
            mainScreen.GetComponent<ShowableView>().Hide();
            game.LoadLevel();
        }

        public void OnLeaderboardBtnClick() {
            authManager.OnLoginSuccess += OnLoginForLeaderboard;
            authManager.Login();
        }

        public void OnAchievementsBtnClick() {
            authManager.OnLoginSuccess += OnLoginForAchievements;
            authManager.Login();
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
            mainScreen.LoginBtn.gameObject.SetActive(false);
        }
    }
}
