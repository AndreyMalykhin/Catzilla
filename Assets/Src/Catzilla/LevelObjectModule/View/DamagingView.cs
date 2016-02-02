using UnityEngine;
using System.Collections;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.Util;
using Catzilla.LevelObjectModule.Model;

namespace Catzilla.LevelObjectModule.View {
    public class DamagingView: strange.extensions.mediation.impl.View {
        public enum Event {TriggerEnter}

        [Inject]
        public EventBus EventBus {get; set;}

        public int Damage = 1;

        private void OnTriggerEnter(Collider collider) {
            ViewUtils.DispatchNowOrAtFixedUpdate(this, GetEventBus,
                Event.TriggerEnter, new Evt(this, collider));
        }

        private EventBus GetEventBus() {
            return EventBus;
        }
    }
}
