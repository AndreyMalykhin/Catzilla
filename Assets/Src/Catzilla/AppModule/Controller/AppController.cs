using UnityEngine;
using System.Collections;
using strange.extensions.context.api;
using GameSparks.Core;
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
            Debug.Log("AppController.OnStart()");
            Server.Connect(OnServerConnectSuccess, OnServerConnectFail);
        }

        private void OnServerConnectSuccess() {
            Server.Login(OnLoginSuccess, OnLoginFail);
        }

        private void OnServerConnectFail() {
            Server.Dispose();
            MainScreen.Show();
        }

        private void OnLoginSuccess() {
            PlayerStateStorage.Sync(Server, OnPlayerSync, OnPlayerSync);
        }

        private void OnLoginFail() {
            MainScreen.Show();
        }

        private void OnPlayerSync() {
            MainScreen.Show();
        }
    }
}
