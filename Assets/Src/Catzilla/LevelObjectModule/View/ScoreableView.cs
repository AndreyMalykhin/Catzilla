using UnityEngine;
using System.Collections;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.Util;

namespace Catzilla.LevelObjectModule.View {
    public class ScoreableView: strange.extensions.mediation.impl.View {
        public enum Event {TriggerEnter}

        [Inject]
        public EventBus EventBus {get; set;}

        public int Score = 1;

        private void OnTriggerEnter(Collider collider) {
            ViewUtils.DispatchNowOrAtFixedUpdate(this, GetEventBus,
                Event.TriggerEnter, new Evt(this, collider));
        }

        private EventBus GetEventBus() {
            return EventBus;
        }
    }
}
