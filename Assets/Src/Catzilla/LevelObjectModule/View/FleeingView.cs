using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;

namespace Catzilla.LevelObjectModule.View {
    public class FleeingView: MonoBehaviour, IPoolable {
        public enum Event {TriggerEnter, TriggerExit}

        public float Speed {
            get {return speed;}
            set {
                speed = value;

                if (animator != null) {
                    animator.SetFloat(speedParam, speed);
                }
            }
        }

        [SerializeField]
        private float minSpeed = 1f;

        [SerializeField]
        private float maxSpeed = 1.25f;

        [SerializeField]
        private float speed;

        private static readonly int speedParam = Animator.StringToHash("Speed");

        [Inject]
        private EventBus eventBus;

        private Rigidbody body;
        private Animator animator;

        void IPoolable.Reset() {
            SetRandomSpeed();
        }

        private void Awake() {
            body = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            SetRandomSpeed();
        }

        private void OnEnable() {
            if (animator != null) {
                animator.SetFloat(speedParam, speed);
            }
        }

        private void FixedUpdate() {
            Vector3 newPosition = transform.position + transform.forward *
                (speed * Time.deltaTime);
            body.MovePosition(newPosition);
        }

        private void OnTriggerEnter(Collider collider) {
            eventBus.Fire(Event.TriggerEnter, new Evt(this, collider));
        }

        private void OnTriggerExit(Collider collider) {
            eventBus.Fire(Event.TriggerExit, new Evt(this, collider));
        }

        private void SetRandomSpeed() {
            Speed = Random.Range(minSpeed, maxSpeed);
        }
    }
}
