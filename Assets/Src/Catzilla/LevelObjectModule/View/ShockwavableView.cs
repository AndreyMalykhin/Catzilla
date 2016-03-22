using UnityEngine;

namespace Catzilla.LevelObjectModule.View {
    public class ShockwavableView: MonoBehaviour {
        public Vector3 CameraShakeAmount = Vector3.up;
        public bool ShakeCameraInOneDirection;

        [Tooltip("In seconds")]
        public float CameraShakeDuration;
    }
}
