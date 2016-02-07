using UnityEngine;
using System.Collections;
using Catzilla.CommonModule.Util;

namespace Catzilla.LevelObjectModule.View {
    public class FleeingView
        : strange.extensions.mediation.impl.View, IPoolable {
        [SerializeField]
        private float minSpeed = 2f;

        [SerializeField]
        private float maxSpeed = 2.5f;

        private Rigidbody body;
        private float speed;

        [PostConstruct]
        public void OnConstruct() {
            Reset();
        }

        public void Reset() {
            speed = Random.Range(minSpeed, maxSpeed);
        }

        protected override void Awake() {
            base.Awake();
            body = GetComponent<Rigidbody>();
        }

        private void OnEnable() {
            // DebugUtils.Log("FleeingView.OnEnable()");
            body.velocity = new Vector3(0f, 0f, speed);
        }

        private void OnDisable() {
            // DebugUtils.Log("FleeingView.OnDisable()");
            body.velocity = Vector3.zero;
        }
    }
}
