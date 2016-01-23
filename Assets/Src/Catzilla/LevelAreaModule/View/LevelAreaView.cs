using UnityEngine;
using System.Collections;
using strange.extensions.context.api;
using strange.extensions.pool.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.View;
using Catzilla.CommonModule.Util;
using Catzilla.LevelAreaModule.Model;
using Catzilla.LevelObjectModule.View;

namespace Catzilla.LevelAreaModule.View {
    public class LevelAreaView: strange.extensions.mediation.impl.View {
        public enum Event {TriggerEnter, TriggerExit}

        [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
        public IEventDispatcher EventBus {get; set;}

        private IEnumerator OnTriggerEnter(Collider collider) {
            yield return new WaitForFixedUpdate();
            EventBus.Dispatch(Event.TriggerEnter, collider);
        }

        private IEnumerator OnTriggerExit(Collider collider) {
            yield return new WaitForFixedUpdate();
            EventBus.Dispatch(Event.TriggerExit, new EventData(this, collider));
        }
    }
}
