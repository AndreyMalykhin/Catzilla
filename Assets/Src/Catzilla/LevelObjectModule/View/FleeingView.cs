using UnityEngine;
using System.Collections;
using strange.extensions.pool.api;

namespace Catzilla.LevelObjectModule.View {
    public class FleeingView
        : strange.extensions.mediation.impl.View, IPoolable {
		public bool retain {get {return false;}}

        [SerializeField]
        private float minSpeed = 2f;

        [SerializeField]
        private float maxSpeed = 2.5f;

        private Rigidbody body;
        private float speed;

        [PostConstruct]
        public void OnConstruct() {
            Restore();
        }

        public void Restore() {
            speed = Random.Range(minSpeed, maxSpeed);
        }

		public void Retain() {Debug.Assert(false);}
		public void Release() {Debug.Assert(false);}

        protected override void Awake() {
            base.Awake();
            body = GetComponent<Rigidbody>();
        }

        private void OnEnable() {
            // Debug.Log("FleeingView.OnEnable()");
            body.velocity = new Vector3(0f, 0f, speed);
        }

        private void OnDisable() {
            // Debug.Log("FleeingView.OnDisable()");
            body.velocity = Vector3.zero;
        }
    }
}
