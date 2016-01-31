using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.Model;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;
using Catzilla.LevelObjectModule.View;
using Catzilla.PlayerModule.Model;
using Catzilla.LeaderboardModule.Model;
using Catzilla.GameOverMenuModule.View;

namespace Catzilla.GameOverMenuModule.Controller {
    public class GameOverMenuController {
        [Inject]
        public GameOverMenuView GameOverMenu {get; set;}

        [Inject]
        public GameOverScreenView GameOverScreen {get; set;}

        [Inject("RewardEarnDlg")]
        public DlgView RewardEarnDlg {get; set;}

        [Inject]
        public PlayerStateStorage PlayerStateStorage {get; set;}

        [Inject]
        public Game Game {get; set;}

        [Inject]
        public Ad Ad {get; set;}

        [Inject]
        public Translator Translator {get; set;}

        [Inject]
        public LeaderboardManager LeaderboardManager {get; set;}

        private PlayerView player;

        [PostConstruct]
        public void OnConstruct() {
            GameOverMenu.ResurrectTextTemplate =
                Translator.Translate("GameOverMenu.Resurrect");
            GameOverMenu.AvailableResurrectionsCount =
                PlayerStateStorage.Get().AvailableResurrectionsCount;
        }

        public void OnServerDispose() {
            GameOverMenu.LeaderboardBtn.interactable = false;
        }

        public void OnLeaderboardBtnClick() {
            AnalyticsUtils.AddEventParam("Origin", "GameOverMenu");
            AnalyticsUtils.AddCategorizedEventParam(
                "Level", PlayerStateStorage.Get().Level);
            AnalyticsUtils.LogEvent("Leaderboard.View");
            LeaderboardManager.Show();
        }

        public void OnPlayerStateStorageSave() {
            GameOverMenu.AvailableResurrectionsCount =
                PlayerStateStorage.Get().AvailableResurrectionsCount;
        }

        public void OnExitBtnClick() {
            AnalyticsUtils.LogEvent("Game.Exit");
            Game.Exit();
        }

        public void OnRestartBtnClick() {
            AnalyticsUtils.AddCategorizedEventParam(
                "Level", PlayerStateStorage.Get().Level);
            AnalyticsUtils.LogEvent("Game.Restart");
            Game.LoadLevel();
            Game.Resume();
            GameOverScreen.Hide();
        }

        public void OnPlayerConstruct(IEvent evt) {
            player = (PlayerView) evt.data;
        }

        public void OnResurrectBtnClick() {
            player.IsHealthFreezed = false;
            player.IsScoreFreezed = false;
            player.Resurrect();
            PlayerState playerState = PlayerStateStorage.Get();
            --playerState.AvailableResurrectionsCount;
            PlayerStateStorage.Save(playerState);
            Game.Resume();
            GameOverScreen.Hide();
        }

        public void OnRewardBtnClick() {
            AnalyticsUtils.AddCategorizedEventParam(
                "Level", PlayerStateStorage.Get().Level);
            AnalyticsUtils.LogEvent("Ad.Show");
            Ad.Show(OnAdView);
        }

        private void OnAdView() {
            AnalyticsUtils.AddCategorizedEventParam(
                "Level", PlayerStateStorage.Get().Level);
            AnalyticsUtils.LogEvent("Ad.View");
            RewardPlayer();
        }

        private void RewardPlayer() {
            PlayerState playerState = PlayerStateStorage.Get();
            ++playerState.AvailableResurrectionsCount;
            PlayerStateStorage.Save(playerState);
            RewardEarnDlg.Show();
        }
    }
}
