using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;

namespace Catzilla.LevelObjectModule.View {
    public class FleeingView: MonoBehaviour, IPoolable {
        public Transform Danger {
            get {return danger;}
            set {
                danger = value;
                desiredSpeed =
                    (value == null) ? 0f : Random.Range(minSpeed, maxSpeed);
                SetSpeed(desiredSpeed);
            }
        }

        private static readonly int speedParam = Animator.StringToHash("Speed");

        [Inject]
        private EventBus eventBus;

        [SerializeField]
        private LayerMask obstacleLayer;

        [SerializeField]
        private float minSpeed;

        [SerializeField]
        private float maxSpeed;

        [SerializeField]
        [Tooltip("In seconds")]
        private float speedAdjustRate;

        [SerializeField]
        private float stopDistance;

        [SerializeField]
        new private Collider collider;

        [SerializeField]
        private Rigidbody body;

        [SerializeField]
        private Animator animator;

        private float speed;
        private float desiredSpeed;
        private Transform danger;
        private Vector3 boundsExtents;
        private float slowDownDistance;
        private float nextAdjustSpeedTime;

        void IPoolable.OnReturn() {
            Danger = null;
        }

		void IPoolable.OnTake() {}

        private void Awake() {
            boundsExtents = collider.bounds.extents;
            slowDownDistance = boundsExtents.z + stopDistance * 2f;
        }

        private void OnEnable() {
            if (animator != null) {
                animator.SetFloat(speedParam, speed);
            }
        }

        private void FixedUpdate() {
            if (danger == null) {
                return;
            }

            float time = Time.time;

            if (time >= nextAdjustSpeedTime) {
                AdjustSpeed();
                nextAdjustSpeedTime = time + speedAdjustRate;
            }

            if (speed == 0f) {
                return;
            }

            Vector3 newPosition = transform.position + transform.forward *
                (speed * Time.deltaTime);
            body.MovePosition(newPosition);
        }

        private void SetSpeed(float value) {
            speed = value;

            if (animator != null && animator.isInitialized) {
                animator.SetFloat(speedParam, speed);
            }
        }

        private void Stop() {
            desiredSpeed = 0f;
            SetSpeed(0f);
            body.Sleep();
        }

        private void AdjustSpeed() {
            Bounds bounds = collider.bounds;

            if (bounds.max.z + stopDistance < danger.position.z) {
                Stop();
                return;
            }

            RaycastHit raycastHit;
            bool isObstacleInFront = Physics.BoxCast(
                bounds.center,
                new Vector3(boundsExtents.x, boundsExtents.y, 0.0625f),
                transform.forward,
                out raycastHit,
                Quaternion.identity,
                slowDownDistance,
                obstacleLayer.value);
            float speedFactor = isObstacleInFront ?
                ((raycastHit.distance - boundsExtents.z - stopDistance) / stopDistance) : 1f;
            speedFactor = Mathf.Max(speedFactor, 0f);
            SetSpeed(Mathf.Lerp(0f, desiredSpeed, speedFactor));
        }

        private void OnDrawGizmos() {
            Gizmos.DrawLine(transform.position,
                transform.position + transform.forward * slowDownDistance);
        }

        private void OnTriggerEnter(Collider collider) {
            Profiler.BeginSample("FleeingView.OnTriggerEnter()");
            eventBus.Fire((int) Events.FleeingTriggerEnter,
                new Evt(this, collider));
            Profiler.EndSample();
        }
    }
}
