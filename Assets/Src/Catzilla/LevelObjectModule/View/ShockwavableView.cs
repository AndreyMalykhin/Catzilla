using UnityEngine;

namespace Catzilla.LevelObjectModule.View {
    public class ShockwavableView: MonoBehaviour {
        public Vector3 CameraShakeAmount {
            get {
                return CameraShakeDirection * UnityEngine.Random.Range(
                    CameraShakeMinAmount, CameraShakeMaxAmount);
            }
        }

        public bool ShakeCameraInOneDirection;
        public float CameraShakeMinAmount;
        public float CameraShakeMaxAmount;
        public Vector3 CameraShakeDirection;

        [Tooltip("In seconds")]
        public float CameraShakeDuration;
    }
}
