using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;

namespace Catzilla.LevelObjectModule.View {
    public class ProjectileView: MonoBehaviour, IPoolable {
        public PoolableView Poolable {get {return poolable;}}

        public float Speed;

        [Inject]
        private EventBus eventBus;

        [Inject]
        private PoolStorageView poolStorage;

        [SerializeField]
        private Rigidbody body;

        [SerializeField]
        private PoolableView poolable;

        private bool isHit;

        public void Hit() {
            if (isHit) {
                return;
            }

            isHit = true;
            poolStorage.ReturnLater(poolable);
        }

        void IPoolable.Reset() {
            isHit = false;
        }

        private void FixedUpdate() {
            Fly();
        }

        private void OnTriggerEnter(Collider collider) {
            eventBus.Fire((int) Events.ProjectileTriggerEnter,
                new Evt(this, collider));
        }

        private void Fly() {
            body.MovePosition(transform.position + transform.forward *
                (Speed * Time.deltaTime));
        }
    }
}
