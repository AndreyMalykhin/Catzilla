using System;
using Zenject;
using Catzilla.PlayerModule.Model;
using Catzilla.CommonModule.View;

namespace Catzilla.CommonModule.Model {
    public class AuthManager {
        public event Action<AuthManager> OnLoginSuccess;

        [Inject]
        private Server server;

        [Inject]
        private PlayerStateStorage playerStateStorage;

        [Inject]
        private UIBlockerView uiBlocker;

        public void Login() {
            if (server.IsLoggedIn) {
                FireLoginSuccessEvent();
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
                            FireLoginSuccessEvent();
                        },
                        () => {
                            showable.Hide();
                            FireLoginSuccessEvent();
                        }
                    );
                },
                () => {
                    showable.Hide();
                }
            );
        }

        private void FireLoginSuccessEvent() {
            if (OnLoginSuccess != null) OnLoginSuccess(this);
        }
    }
}
