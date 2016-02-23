using UnityEngine;
using Zenject;
using Catzilla.CommonModule.Util;

namespace Catzilla.LevelObjectModule.View {
    public class TreatingView: MonoBehaviour {
        public enum Event {TriggerEnter}

        public int AddHealth = 1;

        [Inject]
        private EventBus eventBus;

        private void OnTriggerEnter(Collider collider) {
            eventBus.Fire(Event.TriggerEnter, new Evt(this, collider));
        }
    }
}
