using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;

namespace Catzilla.LevelObjectModule.View {
    public class FleeingView: MonoBehaviour, IPoolable {
        public float Speed {
            get {return desiredSpeed;}
            set {desiredSpeed = value;}
        }

        private static readonly int speedParam = Animator.StringToHash("Speed");

        [SerializeField]
        private LayerMask obstacleLayer;

        [SerializeField]
        private float minSpeed;

        [SerializeField]
        private float maxSpeed;

        [SerializeField]
        private float speed;

        [SerializeField]
        private float desiredSpeed;

        [SerializeField]
        private float stopDistance;

        [SerializeField]
        new private Collider collider;

        [SerializeField]
        private Rigidbody body;

        [SerializeField]
        private Animator animator;

        private float halfDepth;
        private float slowDownDistance;
        private float nextAdjustSpeedTime;

        void IPoolable.OnReturn() {
            InitSpeed();
        }

		void IPoolable.OnTake() {}

        private void Awake() {
            halfDepth = collider.bounds.extents.z;
            slowDownDistance = halfDepth + stopDistance * 2f;
            InitSpeed();
        }

        private void OnEnable() {
            if (animator != null) {
                animator.SetFloat(speedParam, speed);
            }
        }

        private void FixedUpdate() {
            Flee();
        }

        private void InitSpeed() {
            Speed = Random.Range(minSpeed, maxSpeed);
        }

        private void SetSpeed(float value) {
            speed = value;

            if (animator != null) {
                animator.SetFloat(speedParam, speed);
            }
        }

        private void Flee() {
            if (Time.time >= nextAdjustSpeedTime) {
                AdjustSpeed();
                nextAdjustSpeedTime = Time.time + 0.5f;
            }

            Vector3 newPosition = transform.position + transform.forward *
                (speed * Time.deltaTime);
            body.MovePosition(newPosition);
        }

        private void AdjustSpeed() {
            RaycastHit raycastHit;
            bool isObstacleInFront = Physics.BoxCast(
                collider.bounds.center,
                new Vector3(0f, 0f, 0.0625f),
                transform.forward,
                out raycastHit,
                Quaternion.identity,
                slowDownDistance,
                obstacleLayer.value);
            float speedFactor = isObstacleInFront ?
                ((raycastHit.distance - halfDepth - stopDistance) / stopDistance) : 1f;
            speedFactor = Mathf.Max(speedFactor, 0f);
            SetSpeed(Mathf.Lerp(0f, desiredSpeed, speedFactor));
        }

        private void OnDrawGizmos() {
            Gizmos.DrawLine(transform.position,
                transform.position + transform.forward * slowDownDistance);
        }
    }
}
