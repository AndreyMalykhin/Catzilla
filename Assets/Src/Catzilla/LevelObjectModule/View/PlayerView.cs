using UnityEngine;
using System.Collections;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;

namespace Catzilla.LevelObjectModule.View {
    public class PlayerView: LevelObjectView {
        public enum Event {Damaged, Death}

        [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
        public IEventDispatcher EventBus {get; set;}

        [Inject("LevelMinX")]
        public float LevelMinX {get; set;}

        [Inject("LevelMaxX")]
        public float LevelMaxX {get; set;}

        [Inject("EnvLayer")]
        public int EnvLayer {get; set;}

        [SerializeField]
        private float frontSpeed = 5f;

        [SerializeField]
        private float sideSpeed = 5f;

        [SerializeField]
        private float cameraRayLength = 100f;

        [SerializeField]
        private int health = 100;

        private bool isDead = false;
        private float minX;
        private float maxX;
        private float targetX;
        private Rigidbody body;
        private Animator animator;
        new private Camera camera;

        public int Health {get {return health;}}

        [PostConstruct]
        public void OnReady() {
            Debug.Log("PlayerView.OnReady()");
            animator = GetComponentInChildren<Animator>();
            camera = GetComponentInChildren<Camera>();
            body = GetComponent<Rigidbody>();
            targetX = body.position.x;
            float halfWidth =
                GetComponentInChildren<Collider>().bounds.extents.x;
            minX = LevelMinX + halfWidth;
            maxX = LevelMaxX - halfWidth;
        }

        public void TakeDamage(int damage) {
            health = Mathf.Max(0, health - damage);
            EventBus.Dispatch(Event.Damaged);

            if (health > 0) {
                return;
            }

            Die();
        }

        private void Die() {
            isDead = true;
            animator.enabled = false;
            EventBus.Dispatch(Event.Death);
        }

        private void Update() {
            if (Input.GetMouseButtonDown(0)) {
                SetTargetX(Input.mousePosition);
            }
        }

        private void FixedUpdate() {
            if (isDead) {
                return;
            }

            Move();
        }

        private void SetTargetX(Vector3 mousePosition) {
            Ray ray = camera.ScreenPointToRay(mousePosition);
            RaycastHit hit;

            if (!Physics.Raycast(ray, out hit, cameraRayLength, EnvLayer,
                    QueryTriggerInteraction.Collide)) {
                return;
            }

            targetX = Mathf.Clamp(hit.point.x, minX, maxX);
        }

        private void Move() {
            Vector3 currentPosition = body.position;
            float newX = Mathf.MoveTowards(
                currentPosition.x, targetX, sideSpeed * Time.deltaTime);
            float newZ = currentPosition.z + frontSpeed * Time.deltaTime;
            body.MovePosition(new Vector3(newX, currentPosition.y, newZ));
        }
    }
}
