using UnityEngine;
using System.Collections;
using SmartLocalization;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.Util;
using Catzilla.PlayerModule.View;

namespace Catzilla.LevelObjectModule.View {
    public class PlayerView: LevelObjectView {
        public enum Event {Ready, HealthChange, Death, ScoreChange}

        [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
        public IEventDispatcher EventBus {get; set;}

        [Inject("LevelMinX")]
        public float LevelMinX {get; set;}

        [Inject("LevelMaxX")]
        public float LevelMaxX {get; set;}

        [Inject("EnvLayer")]
        public int EnvLayer {get; set;}

        public Collider Collider {get {return collider;}}
        public bool IsHealthFreezed {get; set;}
        public bool IsScoreFreezed {get; set;}

        public int Score {
            get {
                return score;
            }
            set {
                if (IsScoreFreezed) {
                    return;
                }

                score = value;
                EventBus.Dispatch(Event.ScoreChange, this);
            }
        }

        public int Health {
            get {
                return health;
            }
            set {
                if (IsHealthFreezed) {
                    return;
                }

                health = value;
                health = Mathf.Clamp(health, 0, maxHealth);
                EventBus.Dispatch(Event.HealthChange);

                if (health > 0) {
                    return;
                }

                Die();
            }
        }

        [SerializeField]
        private float frontSpeed = 5f;

        [SerializeField]
        private float sideSpeed = 5f;

        [SerializeField]
        private float cameraRayLength = 100f;

        [SerializeField]
        private int maxHealth = 100;

        [SerializeField]
        private Animator animator;

        [SerializeField]
        private PlayerHUDView HUDProto;

        [SerializeField]
        new private Camera camera;

        [SerializeField]
        new private Collider collider;

        private int score;
        private int health;
        private bool isDead;
        private float minX;
        private float maxX;
        private float targetX;
        private Rigidbody body;
        private PlayerHUDView HUD;

        [PostConstruct]
        public void OnReady() {
            Debug.Log("PlayerView.OnReady()");
            body = GetComponent<Rigidbody>();
            targetX = body.position.x;
            float halfWidth = collider.bounds.extents.x;
            minX = LevelMinX + halfWidth;
            maxX = LevelMaxX - halfWidth;
            health = maxHealth;
            HUD = (PlayerHUDView) Instantiate(HUDProto);
            EventBus.Dispatch(Event.Ready, this);
        }

        protected override void OnDestroy() {
            if (HUD != null) {
                Destroy(HUD.gameObject);
            }

            base.OnDestroy();
        }

        private void Die() {
            if (isDead) {
                return;
            }

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
