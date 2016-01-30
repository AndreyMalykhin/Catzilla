using UnityEngine;
using System.Collections;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.Model;
using Catzilla.CommonModule.View;

namespace Catzilla.CommonModule.Controller {
    public class ActivityIndicatorController {
        [Inject]
        public ActivityIndicatorView ActivityIndicator {get; set;}

        public void OnServerRequest() {
            ActivityIndicator.Show();
        }

        public void OnServerResponse(IEvent evt) {
            var server = (Server) evt.data;

            if (server.PendingRequestsCount > 0) {
                return;
            }

            ActivityIndicator.Hide();
        }
    }
}
