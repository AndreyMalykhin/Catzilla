using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;

namespace Catzilla.LevelObjectModule.View {
    public class SmashableView: MonoBehaviour {
        public enum Event {TriggerEnter}

        [Inject]
        public EventBus EventBus {get; set;}

        [Inject]
        public PoolStorageView PoolStorage {get; set;}

        [SerializeField]
        private SmashedView smashedProto;

        private PoolableView poolable;
        private int smashedPoolId;

        [PostInject]
        public void OnConstruct() {
            // DebugUtils.Log("SmashableView.OnConstruct()");
            poolable = GetComponent<PoolableView>();
            smashedPoolId = smashedProto.GetComponent<PoolableView>().PoolId;
        }

        public SmashedView Smash(
            float force, float upwardsModifier, Vector3 sourcePosition) {
            var smashed =
                PoolStorage.Take(smashedPoolId).GetComponent<SmashedView>();
            smashed.Init(this, force, upwardsModifier, sourcePosition);
            PoolStorage.Return(poolable);
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
