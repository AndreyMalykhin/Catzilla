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

        [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
        public IEventDispatcher EventBus {get; set;}

		public bool retain {get {return false;}}

        [SerializeField]
        private MonoBehaviour behaviour;

        [PostConstruct]
        public void OnConstruct() {
            Restore();
        }

        public void Activate() {
            // Debug.Log("ActivatableView.Activate()");
            behaviour.enabled = true;
        }

        public void Restore() {
            behaviour.enabled = false;
        }

		public void Retain() {Debug.Assert(false);}
		public void Release() {Debug.Assert(false);}

        private IEnumerator OnTriggerEnter(Collider collider) {
            yield return new WaitForFixedUpdate();
            EventBus.Dispatch(
                Event.TriggerEnter, new EventData(this, collider));
        }
    }
}
