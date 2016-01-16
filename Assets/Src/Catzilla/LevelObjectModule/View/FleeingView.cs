using UnityEngine;
using System.Collections;

namespace Catzilla.LevelObjectModule.View {
    public class FleeingView: strange.extensions.mediation.impl.View {
        [SerializeField]
        private float minSpeed = 2f;

        [SerializeField]
        private float maxSpeed = 2.5f;

        private Rigidbody body;
        private float speed;

        [PostConstruct]
        public void OnReady() {
            body = GetComponent<Rigidbody>();
            speed = Random.Range(minSpeed, maxSpeed);
        }

        private void FixedUpdate() {
            Move();
        }

        private void Move() {
            Vector3 currentPosition = body.position;
            float newZ = currentPosition.z + speed * Time.deltaTime;
            body.MovePosition(
                new Vector3(currentPosition.x, currentPosition.y, newZ));
        }
    }
}
