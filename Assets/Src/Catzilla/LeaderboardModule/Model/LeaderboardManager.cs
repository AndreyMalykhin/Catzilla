using System.Collections.Generic;
using Facebook.Unity;
using Catzilla.CommonModule.Model;
using Catzilla.LeaderboardModule.View;

namespace Catzilla.LeaderboardModule.Model {
    public class LeaderboardManager {
        [Inject]
        public LeaderboardsScreenView LeaderboardsScreen {get; set;}

        [Inject]
        public Server Server {get; set;}

        public void Show() {
            if (!FB.IsLoggedIn) {
                var permissions = new string[] {
                    "public_profile", "email", "user_friends"};
                FB.LogInWithReadPermissions(permissions, OnFacebookLogin);
                return;
            }

            DoShow();
        }

        private void OnFacebookLogin(ILoginResult loginResult) {
            if (!string.IsNullOrEmpty(loginResult.Error)) {
                // TODO
                return;
            }

            if (!FB.IsLoggedIn) {
                return;
            }

            Server.LinkFacebookAccount(
                loginResult.AccessToken.TokenString, OnFacebookAccountLink);
        }

        private void OnFacebookAccountLink() {
            DoShow();
        }

        private void DoShow() {
            Server.GetScoreLeaderboard(OnScoreLeaderboardGet);
        }

        private void OnScoreLeaderboardGet(List<ScoreLeaderboardItem> items) {
            LeaderboardsScreen.ScoreLeaderboard.Items = items;
            LeaderboardsScreen.Show();
        }
    }
}
