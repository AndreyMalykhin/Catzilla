using UnityEngine;
using System.Collections;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;

namespace Catzilla.LevelObjectModule.View {
    public class SmashableView: strange.extensions.mediation.impl.View {
        public enum Event {TriggerEnter}

        [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
        public IEventDispatcher EventBus {get; set;}

        [Inject]
        public PoolStorageView PoolStorage {get; set;}

        [SerializeField]
        private SmashedView smashedProto;

        private PoolableView poolable;
        private int smashedPoolId;

        [PostConstruct]
        public void OnConstruct() {
            poolable = GetComponent<PoolableView>();
            smashedPoolId = smashedProto.GetComponent<PoolableView>().PoolId;
        }

        public SmashedView Smash(Vector3 sourcePosition) {
            Transform transform = this.transform;
            PoolStorage.Return(poolable);
            var smashed =
                PoolStorage.Get(smashedPoolId).GetComponent<SmashedView>();
            smashed.transform.position = transform.position;
            smashed.transform.rotation = transform.rotation;
            smashed.Init(sourcePosition);
            return smashed;
        }

        private IEnumerator OnTriggerEnter(Collider collider) {
            yield return new WaitForFixedUpdate();
            EventBus.Dispatch(
                Event.TriggerEnter, new EventData(this, collider));
        }
    }
}
