using UnityEngine;
using Zenject;
using Catzilla.CommonModule.Util;

namespace Catzilla.LevelObjectModule.View {
    public class ShockwavableView: MonoBehaviour, IPoolable {
        public bool IsTrigger {get {return isTrigger;}}
        public bool ShakeCameraInOneDirection;
        public float CameraShakeMinAmount;
        public float CameraShakeMaxAmount;
        public Vector3 CameraShakeDirection;

        [Tooltip("In seconds")]
        public float CameraShakeDuration;

        [Inject]
        private EventBus eventBus;

        [SerializeField]
        private bool isTrigger;

        private bool isPropagated;

        public void Propagate() {
            if (isPropagated) {
                return;
            }

            isPropagated = true;
            eventBus.Fire((int) Events.ShockwavablePropagate, new Evt(this));
        }

        void IPoolable.OnReturn() {
            isPropagated = false;
        }

        void IPoolable.OnTake() {}

        private void OnTriggerEnter(Collider collider) {
            eventBus.Fire((int) Events.ShockwavableTriggerEnter,
                new Evt(this, collider));
        }
    }
}
