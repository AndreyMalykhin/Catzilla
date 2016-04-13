using System;
using Zenject;
using Catzilla.PlayerModule.Model;
using Catzilla.LevelObjectModule.View;
using Catzilla.LevelModule.Model;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.Model;

namespace Catzilla.CommonModule.Controller {
    public class AnalyticsController {
        [Inject]
        private PlayerStateStorage playerStateStorage;

        [Inject]
        private LevelSettingsStorage levelSettingsStorage;

        private PlayerView player;

        public void OnPlayerConstruct(Evt evt) {
            player = (PlayerView) evt.Source;
        }

        public void OnPreLevelComplete(Evt evt) {
            AnalyticsUtils.AddEventParam(
                "Player.TotalLifetime", (int) player.TotalLifetime);
            AddPlayerActionsPerMinuteRank();
            AddPlayerDeaths();
            LogEvent("Level.Completion");
        }

        public void OnPlayerDeath(Evt evt) {
            AddPlayerActionsPerMinuteRank();
            AddPlayerLevelCompletionPercentage();
            LogEvent("Player.Death");
        }

        public void OnPlayerResurrect(Evt evt) {
            AddPlayerLevelCompletionPercentage();
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

        public void OnMainScreenExitBtnClick() {
            LogEvent("App.Exit");
        }

        public void OnMainScreenReplaysBtnClick() {
            LogEvent("Replays.View");
        }

        public void OnMainScreenSkillsBtnClick() {
            LogEvent("Skills.View");
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

        public void OnSkillListItemLearnBtnClick(Evt evt) {
            var skillId = (int) evt.Data;
            AnalyticsUtils.AddCategorizedEventParam("Id", skillId);
            LogEvent("Skill.Learn");
        }

        public void OnAppStart(Evt evt) {
            LogEvent("App.Start");
        }

        public void OnAdView(Ad ad) {
            AddPlayerDeaths();
            AddPlayerLevelCompletionPercentage();
            AddPlayerActionsPerMinuteRank();
            LogEvent("Ad.View");
        }

        public void OnLogin(AuthManager authManager) {
            LogEvent("Player.Login");
        }

        private void LogEvent(string name) {
            PlayerState playerState = playerStateStorage.Get();
            AnalyticsUtils.AddCategorizedEventParam(
                "Level", playerState == null ? 0 : playerState.Level);
            AnalyticsUtils.LogEvent(name);
        }

        private void AddPlayerLevelCompletionPercentage() {
            int level = playerStateStorage.Get().Level;
            int levelCompletionScore =
                levelSettingsStorage.Get(level).CompletionScore;
            int levelCompletionPercentage =
                player.Score * 10 / levelCompletionScore;
            AnalyticsUtils.AddCategorizedEventParam(
                "Player.LevelCompletionPercentage", levelCompletionPercentage);
        }

        private void AddPlayerActionsPerMinuteRank() {
            AnalyticsUtils.AddCategorizedEventParam(
                "Player.ActionsPerMinuteRank", player.ActionsPerMinuteRank);
        }

        private void AddPlayerDeaths() {
            AnalyticsUtils.AddEventParam("Player.Deaths", player.DeathsCount);
        }
    }
}
