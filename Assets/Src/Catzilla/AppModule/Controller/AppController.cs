using UnityEngine;
using System;
using System.Collections;
using Zenject;
using Catzilla.MainMenuModule.View;
using Catzilla.PlayerModule.Model;
using Catzilla.CommonModule.Model;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;
using Catzilla.AppModule.Controller;

namespace Catzilla.AppModule.Controller {
    public class AppController {
        [Inject]
        public MainScreenView MainScreen {get; set;}

        [Inject]
        public Server Server {get; set;}

        [Inject]
        public PlayerStateStorage PlayerStateStorage {get; set;}

        [Inject]
        public GiftManager GiftManager {get; set;}

        [Inject]
        public ScreenSpacePopupManagerView PopupManager {get; set;}

        [Inject]
        public Translator Translator {get; set;}

        [Inject("CommonPopupType")]
        public int CommonPopupType {get; set;}

        [Inject]
        public RewardManager RewardManager {get; set;}

        public void OnStart(Evt evt) {
            // DebugUtils.Log("AppController.OnStart()");
            PlayerState playerState = PlayerStateStorage.Get();

            if (playerState == null) {
                playerState = new PlayerState();
                PlayerStateStorage.Save(playerState);
            }

            Server.Connect(OnServerConnectSuccess, OnServerConnectFail);
        }

        private void OnServerConnectSuccess() {
            if (Server.IsLoggedIn) {
                PlayerStateStorage.Sync(Server, OnPlayerSync, OnPlayerSync);
                return;
            }

            ShowMainScreen();
        }

        private void OnServerConnectFail() {
            Server.Dispose();
            ShowMainScreen();
        }

        private void OnPlayerSync() {
            ShowMainScreen();
        }

        private void ShowMainScreen() {
            PlayerState playerState = PlayerStateStorage.Get();

            if (RewardManager.IsTimeToUnlock(playerState)) {
                int unlockedRewardsCount = RewardManager.Unlock(playerState);
                var popup = (ScreenSpacePopupView) PopupManager.Get(
                    CommonPopupType);
                popup.Msg.text = Translator.Translate(
                    "Notification.RewardUnlock", unlockedRewardsCount);
                PopupManager.Show(popup);
            }

            playerState.LastSeenDate = DateTime.UtcNow;
            PlayerStateStorage.Save(playerState);
            MainScreen.GetComponent<ShowableView>().Show();
        }
    }
}
