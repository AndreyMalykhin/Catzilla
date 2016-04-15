using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Model;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;

namespace Catzilla.CommonModule.Controller {
    public class UIBlockerController {
        [Inject]
        private UIBlockerView uiBlocker;

        public void OnServerRequest(Evt evt) {
            uiBlocker.GetComponent<ShowableView>().Show();
        }

        public void OnServerResponse(Evt evt) {
            var server = (Server) evt.Source;

            if (server.PendingRequestsCount > 0) {
                return;
            }

            uiBlocker.GetComponent<ShowableView>().Hide();
        }
    }
}
