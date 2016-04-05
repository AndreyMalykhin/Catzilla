using UnityEngine;
using Zenject;
using Catzilla.CommonModule.Util;

namespace Catzilla.LevelObjectModule.View {
    public class AircraftView: MonoBehaviour {
        [SerializeField]
        private Rigidbody body;

        [SerializeField]
        private float speed;

        private void FixedUpdate() {
            Fly();
        }

        private void Fly() {
            Vector3 newPosition = transform.position + transform.forward *
                (speed * Time.deltaTime);
            body.MovePosition(newPosition);
        }
    }
}
