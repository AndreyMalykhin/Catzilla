using UnityEngine;
using System.Collections;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.Util;

namespace Catzilla.LevelObjectModule.View {
    public class SmashableView: strange.extensions.mediation.impl.View {
        public enum Event {TriggerEnter}

        [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
        public IEventDispatcher EventBus {get; set;}

        [SerializeField]
        private SmashedView smashedProto;

        public void Smash(Vector3 sourcePosition) {
            Transform transform = this.transform;
            Destroy(gameObject);
            var smashedView = (SmashedView) Instantiate(
                smashedProto, transform.position, transform.rotation);
            smashedView.Init(sourcePosition);
        }

        private void OnTriggerEnter(Collider collider) {
            EventBus.Dispatch(
                Event.TriggerEnter, new EventData(this, collider));
        }
    }
}
