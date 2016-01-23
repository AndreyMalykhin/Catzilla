using UnityEngine;
using System.Collections;
using strange.extensions.context.api;
using strange.extensions.pool.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;

namespace Catzilla.LevelObjectModule.View {
    public class ProjectileView: strange.extensions.mediation.impl.View {
        public enum Event {TriggerEnter}

        [Inject]
        public PoolStorageView PoolStorage {get; set;}

        [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
        public IEventDispatcher EventBus {get; set;}

        [SerializeField]
        private float speed = 5f;

        private Rigidbody body;
        private PoolableView poolable;

        [PostConstruct]
        public void OnConstruct() {
            body = GetComponent<Rigidbody>();
            poolable = GetComponent<PoolableView>();
        }

        public void Hit() {
            PoolStorage.Return(poolable);
        }

        private void FixedUpdate() {
            Fly();
        }

        private IEnumerator OnTriggerEnter(Collider collider) {
            yield return new WaitForFixedUpdate();
            EventBus.Dispatch(
                Event.TriggerEnter, new EventData(this, collider));
        }

        private void Fly() {
            body.MovePosition(transform.position + transform.forward *
                (speed * Time.deltaTime));
        }
    }
}
