using UnityEngine;
using System.Collections;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.Util;

namespace Catzilla.CommonModule.View {
    public class DisposableView: strange.extensions.mediation.impl.View {
        public enum Event {TriggerExit}

        [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
        public IEventDispatcher EventBus {get; set;}

        [Inject]
        public PoolStorageView PoolStorage {get; set;}

        [SerializeField]
        private PoolableView root;

        private PoolableView poolable;

        [PostConstruct]
        public void OnConstruct() {
            poolable = root == null ? GetComponent<PoolableView>() : root;
        }

        public void Dispose() {
            PoolStorage.Return(poolable);
        }

        private IEnumerator OnTriggerExit(Collider collider) {
            yield return new WaitForFixedUpdate();
            EventBus.Dispatch(Event.TriggerExit, new EventData(this, collider));
        }
    }
}
