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

        public void Login() {
            if (server.IsLoggedIn) {
                FireLoginSuccessEvent();
                return;
            }

            server.Login(
                () => {
                    playerStateStorage.Sync(
                        server,
                        () => {
                            FireLoginSuccessEvent();
                        },
                        () => {
                            FireLoginSuccessEvent();
                        }
                    );
                }
            );
        }

        private void FireLoginSuccessEvent() {
            if (OnLoginSuccess != null) OnLoginSuccess(this);
        }
    }
}
