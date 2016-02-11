using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Model;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;

namespace Catzilla.CommonModule.Controller {
    public class ActivityIndicatorController {
        [Inject]
        public ActivityIndicatorView ActivityIndicator {get; set;}

        public void OnServerRequest(Evt evt) {
            ActivityIndicator.Show();
        }

        public void OnServerResponse(Evt evt) {
            var server = (Server) evt.Source;

            if (server.PendingRequestsCount > 0) {
                return;
            }

            ActivityIndicator.Hide();
        }
    }
}
