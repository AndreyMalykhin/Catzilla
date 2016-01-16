using UnityEngine;
using System.Collections;

namespace Catzilla.LevelObjectModule.View {
    public class ProjectileView: strange.extensions.mediation.impl.View {
        [SerializeField]
        private float speed = 5f;

        private Rigidbody body;

        [PostConstruct]
        public void OnReady() {
            body = GetComponent<Rigidbody>();
        }

        private void FixedUpdate() {
            Fly();
        }

        private void Fly() {
            body.MovePosition(transform.position + transform.forward *
                (speed * Time.deltaTime));
        }
    }
}
