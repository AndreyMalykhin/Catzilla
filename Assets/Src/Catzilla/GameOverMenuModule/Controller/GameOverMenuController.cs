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
    public class GameOverMenuController {
        [Inject]
        public GameOverMenuView GameOverMenu {get; set;}

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

        [Inject("PlayStopwatch")]
        public Stopwatch PlayStopwatch {get; set;}

        [Inject]
        public ScreenSpacePopupManagerView PopupManager {get; set;}

        [Inject("MainCamera")]
        public Camera MainCamera {get; set;}

        private PlayerView player;

        [PostInject]
        public void OnConstruct() {
            GameOverMenu.ResurrectTextTemplate =
                Translator.Translate("GameOverMenu.Resurrect");
            PlayerState playerState = PlayerStateStorage.Get();

            if (playerState != null) {
                GameOverMenu.AvailableResurrectionsCount =
                    playerState.AvailableResurrectionsCount;
            }
        }

        public void OnPlayerStateStorageSave(Evt evt) {
            GameOverMenu.AvailableResurrectionsCount =
                PlayerStateStorage.Get().AvailableResurrectionsCount;
        }

        public void OnExitBtnClick(Evt evt) {
            Game.UnloadLevel();
            MainCamera.gameObject.SetActive(true);
            GameOverScreen.Hide();
            MainScreen.Show();
        }

        public void OnRestartBtnClick(Evt evt) {
            AnalyticsUtils.AddCategorizedEventParam(
                "Level", PlayerStateStorage.Get().Level);
            AnalyticsUtils.LogEvent("Game.Restart");
            Game.LoadLevel();
            GameOverScreen.Hide();
        }

        public void OnPlayerConstruct(Evt evt) {
            player = (PlayerView) evt.Source;
        }

        public void OnResurrectBtnClick(Evt evt) {
            player.IsHealthFreezed = false;
            player.IsScoreFreezed = false;
            player.Resurrect();
            PlayerState playerState = PlayerStateStorage.Get();
            --playerState.AvailableResurrectionsCount;
            PlayerStateStorage.Save(playerState);
            PlayStopwatch.Reset();
            PlayStopwatch.Start();
            Game.Resume();
            GameOverScreen.Hide();
        }

        public void OnRewardBtnClick(Evt evt) {
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
            int addResurrectionsCount =
                LevelSettingsStorage.Get(playerState.Level).ResurrectionReward;
            playerState.AvailableResurrectionsCount += addResurrectionsCount;
            PlayerStateStorage.Save(playerState);
            ScreenSpacePopupView popup = PopupManager.Get();
            popup.Msg.text = Translator.Translate(
                "Player.RewardEarn", addResurrectionsCount);
            PopupManager.Show(popup);
        }
    }
}
