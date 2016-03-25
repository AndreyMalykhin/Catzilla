using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;

namespace Catzilla.LevelObjectModule.View {
    public class ActivatableView: MonoBehaviour, IPoolable {
        [Inject]
        public EventBus EventBus {get; set;}

        [SerializeField]
        private MonoBehaviour behaviour;

        [PostInject]
        public void OnConstruct() {
            behaviour.enabled = false;
        }

        public void Activate() {
            // DebugUtils.Log("ActivatableView.Activate()");
            behaviour.enabled = true;
        }

        void IPoolable.Reset() {
            behaviour.enabled = false;
        }

        private void OnTriggerEnter(Collider collider) {
            EventBus.Fire((int) Events.ActivatableTriggerEnter,
                new Evt(this, collider));
        }
    }
}
