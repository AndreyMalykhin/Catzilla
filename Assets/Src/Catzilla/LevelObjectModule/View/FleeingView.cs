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

        [Inject("LevelObjectLayer")]
        private int levelObjectLayer;

        [Inject("LevelObjectSmashedLayer")]
        private int levelObjectSmashedLayer;

        [SerializeField]
        private float minSpeed = 1f;

        [SerializeField]
        private float maxSpeed = 1.25f;

        [SerializeField]
        private float speed;

        [SerializeField]
        private float desiredSpeed;

        [SerializeField]
        private float stopDistance = 0.5f;

        [SerializeField]
        new private Collider collider;

        [SerializeField]
        private Rigidbody body;

        [SerializeField]
        private Animator animator;

        private float halfDepth;
        private float slowDownDistance;

        void IPoolable.Reset() {
            InitSpeed();
        }

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
            RaycastHit raycastHit;
            bool isObstacleInFront = Physics.Raycast(
                transform.position,
                transform.forward,
                out raycastHit,
                slowDownDistance,
                levelObjectLayer | levelObjectSmashedLayer);
            float speedFactor = isObstacleInFront ?
                ((raycastHit.distance - halfDepth - stopDistance) / stopDistance) : 1f;
            SetSpeed(Mathf.Lerp(0f, desiredSpeed, speedFactor));
            Vector3 newPosition = transform.position + transform.forward *
                (speed * Time.deltaTime);
            body.MovePosition(newPosition);
        }
    }
}
