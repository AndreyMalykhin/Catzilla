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

            var showable = uiBlocker.GetComponent<ShowableView>();
            showable.Show();
            server.Login(
                () => {
                    playerStateStorage.Sync(
                        server,
                        () => {
                            showable.Hide();
                            if (onSuccess != null) onSuccess();
                        },
                        () => {
                            showable.Hide();
                            if (onSuccess != null) onSuccess();
                        }
                    );
                },
                () => {
                    showable.Hide();
                }
            );
        }
    }
}
