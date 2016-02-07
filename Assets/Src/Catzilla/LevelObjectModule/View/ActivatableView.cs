using UnityEngine;
using System.Collections;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.Util;

namespace Catzilla.LevelObjectModule.View {
    public class ActivatableView
        : strange.extensions.mediation.impl.View, IPoolable {
        public enum Event {TriggerEnter}

        [Inject]
        public EventBus EventBus {get; set;}

        [SerializeField]
        private MonoBehaviour behaviour;

        [PostConstruct]
        public void OnConstruct() {
            Reset();
        }

        public void Activate() {
            // DebugUtils.Log("ActivatableView.Activate()");
            behaviour.enabled = true;
        }

        public void Reset() {
            behaviour.enabled = false;
        }

        private void OnTriggerEnter(Collider collider) {
            ViewUtils.DispatchNowOrAtFixedUpdate(this, GetEventBus,
                Event.TriggerEnter, new Evt(this, collider));
        }

        private EventBus GetEventBus() {
            return EventBus;
        }
    }
}
