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
            body = GetComponent<Rigidbody>();
            Restore();
        }

        public void Restore() {
            speed = Random.Range(minSpeed, maxSpeed);
            body.velocity = new Vector3(0f, 0f, speed);
        }

		public void Retain() {Debug.Assert(false);}
		public void Release() {Debug.Assert(false);}
    }
}
