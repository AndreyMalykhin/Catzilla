using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Diagnostics;
using Zenject;
using Catzilla.CommonModule.Model;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;
using Catzilla.LevelObjectModule.View;
using Catzilla.LevelModule.Model;
using Catzilla.PlayerModule.Model;
using Catzilla.MainMenuModule.View;
using Catzilla.GameOverMenuModule.View;

namespace Catzilla.GameOverMenuModule.Controller {
    public class GameOverScreenController {
        [Inject]
        public GameOverScreenView GameOverScreen {get; set;}

        [Inject]
        public MainScreenView MainScreen {get; set;}

        [Inject]
        public PlayerStateStorage PlayerStateStorage {get; set;}

        [Inject]
        public LevelSettingsStorage LevelSettingsStorage {get; set;}

        [Inject]
        public Game Game {get; set;}

        [Inject]
        public Ad Ad {get; set;}

        [Inject]
        public Translator Translator {get; set;}

        [Inject]
        public ScreenSpacePopupManagerView PopupManager {get; set;}

        [Inject("CommonPopupType")]
        public int CommonPopupType {get; set;}

        [Inject]
        public RewardManager RewardManager {get; set;}

        private PlayerView player;

        [PostInject]
        public void OnConstruct() {
            GameOverScreen.Menu.ResurrectTextTemplate =
                Translator.Translate("GameOverMenu.Resurrect");
            GameOverScreen.Menu.RewardTextTemplate =
                Translator.Translate("GameOverMenu.Reward");
            PlayerState playerState = PlayerStateStorage.Get();

            if (playerState != null) {
                GameOverScreen.Menu.AvailableResurrectionsCount =
                    playerState.AvailableResurrectionsCount;
                GameOverScreen.Menu.AvailableRewardsCount =
                    playerState.AvailableRewardsCount;
            }
        }

        public void OnPlayerStateStorageSave(Evt evt) {
            PlayerState playerState = PlayerStateStorage.Get();
            GameOverScreen.Menu.AvailableResurrectionsCount =
                playerState.AvailableResurrectionsCount;
            GameOverScreen.Menu.AvailableRewardsCount =
                playerState.AvailableRewardsCount;
        }

        public void OnExitBtnClick() {
            AnalyticsUtils.LogEvent("Game.Exit");
            Game.UnloadLevel();
            GameOverScreen.GetComponent<ShowableView>().Hide();
            MainScreen.GetComponent<ShowableView>().Show();
        }

        public void OnRestartBtnClick() {
            AnalyticsUtils.AddCategorizedEventParam(
                "Level", PlayerStateStorage.Get().Level);
            AnalyticsUtils.LogEvent("Game.Restart");
            var showable = GameOverScreen.GetComponent<ShowableView>();
            showable.OnHide += OnHideForRestart;
            showable.Hide();
        }

        public void OnPlayerConstruct(Evt evt) {
            player = (PlayerView) evt.Source;
        }

        public void OnResurrectBtnClick() {
            PlayerState playerState = PlayerStateStorage.Get();
            --playerState.AvailableResurrectionsCount;
            PlayerStateStorage.Save(playerState);
            player.IsHealthFreezed = false;
            player.IsScoreFreezed = false;
            player.Resurrect();
            GameOverScreen.GetComponent<ShowableView>().Hide();
        }

        public void OnRewardBtnClick() {
            AnalyticsUtils.AddCategorizedEventParam(
                "Level", PlayerStateStorage.Get().Level);
            AnalyticsUtils.LogEvent("Ad.Show");
            Ad.Show(OnAdView);
        }

        private void OnAdView() {
            PlayerState playerState = PlayerStateStorage.Get();
            AnalyticsUtils.AddCategorizedEventParam("Level", playerState.Level);
            AnalyticsUtils.LogEvent("Ad.View");
            RewardPlayer(playerState);
        }

        private void RewardPlayer(PlayerState playerState) {
            int addResurrectionsCount = RewardManager.Give(playerState);
            PlayerStateStorage.Save(playerState);
            ScreenSpacePopupView popup = PopupManager.Get(CommonPopupType);
            popup.Msg.text = Translator.Translate(
                "Player.RewardEarn", addResurrectionsCount);
            PopupManager.Show(popup);
        }

        private void OnHideForRestart(ShowableView showable) {
            showable.OnHide -= OnHideForRestart;
            Game.LoadLevel();
        }
    }
}
