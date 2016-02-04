using UnityEngine;
using System.Collections;
using strange.extensions.context.api;
using GameSparks.Core;
using Facebook.Unity;
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

            FB.Init(OnFacebookInit);
        }

        private void OnFacebookInit() {
            if (FB.IsInitialized) {
                FB.ActivateApp();
            }

            Server.Connect(OnServerConnectSuccess, OnServerConnectFail);
        }

        private void OnServerConnectSuccess() {
            Server.Login("anonymous", OnLoginSuccess, OnLoginFail);
        }

        private void OnServerConnectFail() {
            Server.Dispose();
            ShowMainScreen();
        }

        private void OnLoginSuccess() {
            PlayerStateStorage.Sync(Server, OnPlayerSync, OnPlayerSync);
        }

        private void OnLoginFail() {
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
            playerState.AvailableResurrectionsCount =
                Mathf.Max(playerState.AvailableResurrectionsCount, 1);
        }
    }
}
