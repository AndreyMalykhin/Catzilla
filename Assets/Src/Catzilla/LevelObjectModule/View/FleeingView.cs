using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;

namespace Catzilla.LevelObjectModule.View {
    public class FleeingView: MonoBehaviour, IPoolable {
        public float speed;

        [SerializeField]
        private float minSpeed = 1f;

        [SerializeField]
        private float maxSpeed = 1.25f;

        private Rigidbody body;
        private Animator animator;

        [PostInject]
        public void OnConstruct() {
            SetSpeed();
        }

        void IPoolable.Reset() {
            SetSpeed();
        }

        private void Awake() {
            body = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
        }

        private void OnEnable() {
            // DebugUtils.Log("FleeingView.OnEnable()");
            body.velocity = new Vector3(0f, 0f, speed);

            if (animator != null) {
                animator.SetFloat("Speed", speed);
            }
        }

        private void OnDisable() {
            // DebugUtils.Log("FleeingView.OnDisable()");
            body.velocity = Vector3.zero;
        }

        private void SetSpeed() {
            speed = Random.Range(minSpeed, maxSpeed);
        }
    }
}
