using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;

namespace Catzilla.LevelObjectModule.View {
    public class ProjectileView: MonoBehaviour {
        public enum Event {TriggerEnter}

        public PoolableView Poolable {get {return poolable;}}

        public float Speed;

        [Inject]
        private EventBus eventBus;

        [SerializeField]
        private Rigidbody body;

        [SerializeField]
        private PoolableView poolable;

        private void FixedUpdate() {
            Fly();
        }

        private void OnTriggerEnter(Collider collider) {
            ViewUtils.DispatchNowOrAtFixedUpdate(this, GetEventBus,
                Event.TriggerEnter, new Evt(this, collider));
        }

        private EventBus GetEventBus() {
            return eventBus;
        }

        private void Fly() {
            body.MovePosition(transform.position + transform.forward *
                (Speed * Time.deltaTime));
        }
    }
}
