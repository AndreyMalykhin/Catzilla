using UnityEngine;
using System.Collections.Generic;
using Zenject;
using Catzilla.CommonModule.Model;
using Catzilla.CommonModule.Util;

namespace Catzilla.PlayerModule.Model {
    public class LeaderboardManager {
        [Inject]
        public Server Server {get; set;}

        [Inject]
        public PlayerStateStorage PlayerStateStorage {get; set;}

        public void Show() {
            if (!Server.IsLoggedIn) {
                Server.Login(OnLoginSuccess, OnLoginFail);
                return;
            }

            DoShow();
        }

        private void OnLoginSuccess() {
            PlayerStateStorage.Sync(Server, OnSyncSuccess, OnSyncFail);
        }

        private void OnLoginFail() {
            // DebugUtils.Log("LeaderboardManager.OnLoginFail()");
        }

        private void OnSyncSuccess() {
            DoShow();
        }

        private void OnSyncFail() {
            // DebugUtils.Log("LeaderboardManager.OnSyncFail()");
        }

        private void DoShow() {
            Social.ShowLeaderboardUI();
        }
    }
}
