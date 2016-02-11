using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;

namespace Catzilla.LevelObjectModule.View {
    public class ActivatableView
        : MonoBehaviour, IPoolable {
        public enum Event {TriggerEnter}

        [Inject]
        public EventBus EventBus {get; set;}

        [SerializeField]
        private MonoBehaviour behaviour;

        [PostInject]
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
