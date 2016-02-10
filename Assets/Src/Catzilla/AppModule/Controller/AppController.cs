using UnityEngine;
using System;
using System.Collections;
using strange.extensions.context.api;
using Catzilla.MainMenuModule.View;
using Catzilla.PlayerModule.Model;
using Catzilla.CommonModule.Model;
using Catzilla.AppModule.Controller;

namespace Catzilla.AppModule.Controller {
    public class AppController {
        [Inject]
        public MainScreenView MainScreen {get; set;}

        [Inject]
        public Server Server {get; set;}

        [Inject]
        public PlayerStateStorage PlayerStateStorage {get; set;}

        public void OnStart() {
            // DebugUtils.Log("AppController.OnStart()");
            if (PlayerStateStorage.Get() == null) {
                PlayerStateStorage.Save(new PlayerState());
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
            MainScreen.Show();
            RewardPlayer();
        }

        private void RewardPlayer() {
            PlayerState playerState = PlayerStateStorage.Get();

            if (playerState.AvailableResurrectionsCount <= 0) {
                playerState.AvailableResurrectionsCount = 1;
            }
        }
    }
}
