using UnityEngine;

namespace Catzilla.CommonModule.View {
    public class ValueShakerView: MonoBehaviour {
        public Vector3 Amount {
            get {return IsShaking ? amount : Vector3.zero;}
        }

        public bool IsShaking {get {return Time.time < stopTime;}}

        [SerializeField]
        private float noiseFactor;

        private Vector3 maxNoise;
        private bool inOneDirection;
        private float sign = 1f;
        private float stopTime;
        private float duration;
        private Vector3 amount;
        private Vector3 startAmount;

        public void Shake(Vector3 amount, float duration, bool inOneDirection) {
            this.inOneDirection = inOneDirection;
            this.startAmount = amount;
            this.duration = duration;
            stopTime = Time.time + duration;
            maxNoise = amount * noiseFactor;
        }

        private void Update() {
            if (!IsShaking) {
                return;
            }

            float time = Time.time;
            Vector3 noise =
                maxNoise * (Mathf.PerlinNoise(time, 0f) - 0.5f);
            float remainingTime = stopTime - time;
            amount =
                (startAmount * (sign * (remainingTime / duration))) + noise;

            if (!inOneDirection) {
                sign *= -1f;
            }
        }
    }
}
