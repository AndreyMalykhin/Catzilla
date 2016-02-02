using UnityEngine;
using System.Collections;
using strange.extensions.context.api;
using strange.extensions.pool.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.Util;

namespace Catzilla.LevelObjectModule.View {
    public class ActivatableView
        : strange.extensions.mediation.impl.View, IPoolable {
        public enum Event {TriggerEnter}

        [Inject]
        public EventBus EventBus {get; set;}

		public bool retain {get {return false;}}

        [SerializeField]
        private MonoBehaviour behaviour;

        [PostConstruct]
        public void OnConstruct() {
            Restore();
        }

        public void Activate() {
            // DebugUtils.Log("ActivatableView.Activate()");
            behaviour.enabled = true;
        }

        public void Restore() {
            behaviour.enabled = false;
        }

		public void Retain() {Debug.Assert(false);}
		public void Release() {Debug.Assert(false);}

        private void OnTriggerEnter(Collider collider) {
            ViewUtils.DispatchNowOrAtFixedUpdate(this, GetEventBus,
                Event.TriggerEnter, new Evt(this, collider));
        }

        private EventBus GetEventBus() {
            return EventBus;
        }
    }
}
