using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Model;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;

namespace Catzilla.CommonModule.Controller {
    public class ActivityIndicatorController {
        [Inject]
        private ActivityIndicatorView activityIndicator;

        public void OnServerRequest(Evt evt) {
            activityIndicator.GetComponent<ShowableView>().Show();
        }

        public void OnServerResponse(Evt evt) {
            var server = (Server) evt.Source;

            if (server.PendingRequestsCount > 0) {
                return;
            }

            activityIndicator.GetComponent<ShowableView>().Hide();
        }
    }
}
