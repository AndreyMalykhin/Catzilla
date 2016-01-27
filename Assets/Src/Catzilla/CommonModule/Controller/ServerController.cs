using UnityEngine;
using System.Collections;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.Model;
using Catzilla.CommonModule.View;

namespace Catzilla.CommonModule.Controller {
    public class ServerController {
        [Inject]
        public ActivityIndicatorView ActivityIndicator {get; set;}

        public void OnRequest() {
            ActivityIndicator.Show();
        }

        public void OnResponse(IEvent evt) {
            var server = (Server) evt.data;

            if (server.PendingRequestsCount > 0) {
                return;
            }

            ActivityIndicator.Hide();
        }
    }
}
