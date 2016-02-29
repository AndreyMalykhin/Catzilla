using System;
using Zenject;
using Catzilla.PlayerModule.Model;
using Catzilla.CommonModule.View;

namespace Catzilla.CommonModule.Model {
    public class AuthManager {
        [Inject]
        private Server server;

        [Inject]
        private PlayerStateStorage playerStateStorage;

        [Inject]
        private UIBlockerView uiBlocker;

        public void Login(Action onSuccess = null) {
            if (server.IsLoggedIn) {
                if (onSuccess != null) onSuccess();
                return;
            }

            server.Login(() => {
                uiBlocker.Show();
                playerStateStorage.Sync(
                    server,
                    () => {
                        uiBlocker.Hide();
                        if (onSuccess != null) onSuccess();
                    },
                    () => {
                        uiBlocker.Hide();
                        if (onSuccess != null) onSuccess();
                    }
                );
            });
        }
    }
}
