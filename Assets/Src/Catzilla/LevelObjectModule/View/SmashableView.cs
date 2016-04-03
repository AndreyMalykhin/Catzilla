using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;

namespace Catzilla.LevelObjectModule.View {
    public class SmashableView: MonoBehaviour, IPoolable {
        public bool IsSmashed {get {return isSmashed;}}
        public Transform[] Parts {get {return parts;}}

        [Inject]
        private EventBus eventBus;

        [Inject]
        private PoolStorageView poolStorage;

        [SerializeField]
        private SmashedView smashedProto;

        [SerializeField]
        private Transform[] parts;

        [SerializeField]
        private PoolableView poolable;

        private bool isSmashed;
        private int smashedPoolId;

        [PostInject]
        public void OnConstruct() {
            // DebugUtils.Log("SmashableView.OnConstruct()");
            smashedPoolId = smashedProto.GetComponent<PoolableView>().PoolId;
        }

        public void Smash(
            float force, float upwardsModifier, Vector3 sourcePosition) {
            if (isSmashed) {
                return;
            }

            Profiler.BeginSample("SmashableView.Smash()");
            var smashed =
                poolStorage.Take(smashedPoolId).GetComponent<SmashedView>();
            smashed.Init(this, force, upwardsModifier, sourcePosition);
            isSmashed = true;
            eventBus.Fire((int) Events.SmashableSmash, new Evt(this, smashed));
            poolStorage.ReturnLater(poolable);
            Profiler.EndSample();
        }

        void IPoolable.OnReturn() {
            isSmashed = false;
        }

		void IPoolable.OnTake() {}

        private void OnTriggerEnter(Collider collider) {
            Profiler.BeginSample("SmashableView.OnTriggerEnter()");
            eventBus.Fire((int) Events.SmashableTriggerEnter,
                new Evt(this, collider));
            Profiler.EndSample();
        }

        // private void OnCollisionEnter(Collision collision) {
        //     eventBus.Fire((int) Events.SmashableCollisionEnter,
        //         new Evt(this, collision));
        // }
    }
}
