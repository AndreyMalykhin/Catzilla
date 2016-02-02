using UnityEngine;
using System.Collections;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;

namespace Catzilla.LevelObjectModule.View {
    public class SmashableView: strange.extensions.mediation.impl.View {
        public enum Event {TriggerEnter}

        [Inject]
        public EventBus EventBus {get; set;}

        [Inject]
        public PoolStorageView PoolStorage {get; set;}

        [SerializeField]
        private SmashedView smashedProto;

        private PoolableView poolable;
        private int smashedPoolId;

        [PostConstruct]
        public void OnConstruct() {
            // DebugUtils.Log("SmashableView.OnConstruct()");
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

        private void OnTriggerEnter(Collider collider) {
            ViewUtils.DispatchNowOrAtFixedUpdate(this, GetEventBus,
                Event.TriggerEnter, new Evt(this, collider));
        }

        private EventBus GetEventBus() {
            return EventBus;
        }
    }
}
