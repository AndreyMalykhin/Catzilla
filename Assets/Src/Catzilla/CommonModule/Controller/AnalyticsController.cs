using Zenject;
using Catzilla.PlayerModule.Model;
using Catzilla.LevelObjectModule.View;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.Model;

namespace Catzilla.CommonModule.Controller {
    public class AnalyticsController {
        [Inject]
        private PlayerStateStorage playerStateStorage;

        public void OnPreLevelComplete(Evt evt) {
            var player = (PlayerView) evt.Data;
            AnalyticsUtils.AddCategorizedEventParam(
                "ActionsPerMinuteRank", player.ActionsPerMinuteRank);
            LogEvent("Level.Completion");
        }

        public void OnPlayerDeath(Evt evt) {
            var player = (PlayerView) evt.Source;
            AnalyticsUtils.AddCategorizedEventParam(
                "ActionsPerMinuteRank", player.ActionsPerMinuteRank);
            LogEvent("Player.Death");
        }

        public void OnPlayerResurrect(Evt evt) {
            LogEvent("Player.Resurrection");
        }

        public void OnMainScreenFeedbackBtnClick() {
            LogEvent("Feedback.Leave");
        }

        public void OnMainScreenStartBtnClick() {
            LogEvent("Game.Start");
        }

        public void OnMainScreenLeaderboardBtnClick() {
            LogEvent("Leaderboard.View");
        }

        public void OnMainScreenAchievementsBtnClick() {
            LogEvent("Achievements.View");
        }

        public void OnMainScreenReplaysBtnClick() {
            LogEvent("Replays.View");
        }

        public void OnGameOverScreenExitBtnClick() {
            LogEvent("Game.Exit");
        }

        public void OnGameOverScreenRestartBtnClick() {
            LogEvent("Game.Restart");
        }

        public void OnLevelCompleteScreenShareBtnClick() {
            LogEvent("Replay.Share");
        }

        public void OnAdView(Ad ad) {
            LogEvent("Ad.View");
        }

        public void OnLogin(AuthManager authManager) {
            LogEvent("Player.Login");
        }

        private void LogEvent(string name) {
            PlayerState playerState = playerStateStorage.Get();
            AnalyticsUtils.AddCategorizedEventParam("Level", playerState.Level);
            AnalyticsUtils.LogEvent(name);
        }
    }
}
