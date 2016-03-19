using UnityEngine;
using UnityEngine.Serialization;
using System.Collections;
using SmartLocalization;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.PlayerModule.View;

namespace Catzilla.LevelObjectModule.View {
    public class PlayerView: LevelObjectView {
        public enum Event {
            Construct,
            HealthChange,
            Death,
            ScoreChange,
            Resurrect,
            Footstep,
            Destroy
        }

        [Inject]
        public EventBus EventBus {get; set;}

        [Inject("LevelMinX")]
        public float LevelMinX {get; set;}

        [Inject("LevelMaxX")]
        public float LevelMaxX {get; set;}

        [Inject]
        public IInstantiator Instantiator {get; set;}

        public int ScoreBonusesTaken {get; set;}

        public int Score {
            get {return score;}
            set {
                // DebugUtils.Log("PlayerView.Score set");

                if (IsScoreFreezed || score == value) {
                    return;
                }

                score = value;
                EventBus.Fire(Event.ScoreChange, new Evt(this));
            }
        }

        public int Health {
            get {return health;}
            set {
                if (IsHealthFreezed || health == value) {
                    return;
                }

                int oldValue = health;
                health = value;
                health = Mathf.Clamp(health, 0, MaxHealth);
                EventBus.Fire(
                    Event.HealthChange, new Evt(this, oldValue));

                if (health <= 0) {
                    Die();
                }
            }
        }

        public Collider Collider;
        public Camera Camera;
        public HUDView HUD;
        public Animator Animator;
        public AudioClip DeathSound;
        public AudioClip HurtSound;
        public AudioClip TreatSound;
        public AudioClip FootstepSound;
        public AudioSource LowPrioAudioSource;
        public AudioSource HighPrioAudioSource;
        public bool IsHealthFreezed;
        public bool IsScoreFreezed;
        public float FrontSpeed = 5f;
        public float SideSpeed = 5f;
        public int MaxHealth = 100;
        public float CameraShakeDuration = 0.25f;
        public Vector3 CameraShakeMaxAmount = Vector3.up;

        [SerializeField]
        private HUDView hudProto;

        [SerializeField]
        private LayerMask envLayer;

        [SerializeField]
        private bool shakeCameraInOneDirection;

        private Vector3 cameraShakeAmount;
        private IEnumerator cameraShakeCoroutine;
        private Vector3 cameraStartPosition;
        private int score;
        private int health;
        private bool isDead;
        private float minX;
        private float maxX;
        private float targetX;
        private float nextFootstepTime;
        private Rigidbody body;

        [PostInject]
        public void OnConstruct() {
            // DebugUtils.Log("PlayerView.OnConstruct()");
            body = GetComponent<Rigidbody>();
            float halfWidth = Collider.bounds.extents.x;
            minX = LevelMinX + halfWidth;
            maxX = LevelMaxX - halfWidth;
            health = MaxHealth;
            cameraStartPosition = Camera.transform.localPosition;
            HUD = Instantiator.InstantiatePrefab(hudProto.gameObject)
                .GetComponent<HUDView>();
            EventBus.Fire(Event.Construct, new Evt(this));
        }

        public void Resurrect() {
            if (!isDead) {
                return;
            }

            isDead = false;
            targetX = body.position.x;
            Animator.enabled = true;
            Health = MaxHealth;
            EventBus.Fire(Event.Resurrect, new Evt(this));
        }

        private void OnDestroy() {
            if (HUD != null) {
                Destroy(HUD.gameObject);
            }

            EventBus.Fire(Event.Destroy, new Evt(this));
        }

        private void Die() {
            if (isDead) {
                return;
            }

            isDead = true;
            Animator.enabled = false;
            EventBus.Fire(Event.Death, new Evt(this));
        }

        private void Update() {
            if (isDead) {
                return;
            }

            if (Input.GetMouseButtonDown(0)) {
                SetTargetX(Input.mousePosition);
            }
        }

        private void LateUpdate() {
            MoveCamera();
        }

        private void FixedUpdate() {
            if (isDead) {
                return;
            }

            Move();
        }

        private void Start() {
            targetX = body.position.x;
        }

        private void SetTargetX(Vector3 mousePosition) {
            Ray ray = Camera.ScreenPointToRay(mousePosition);
            RaycastHit hit;

            if (!Physics.Raycast(
                    ray, out hit, Camera.farClipPlane, envLayer.value)) {
                return;
            }

            targetX = Mathf.Clamp(hit.point.x, minX, maxX);
        }

        private void Move() {
            Vector3 currentPosition = body.position;
            float newX = Mathf.MoveTowards(
                currentPosition.x, targetX, SideSpeed * Time.deltaTime);
            float newZ = currentPosition.z + FrontSpeed * Time.deltaTime;
            body.MovePosition(new Vector3(newX, currentPosition.y, newZ));
        }

        private void MoveCamera() {
            var newPosition = new Vector3(
                cameraStartPosition.x - transform.position.x,
                cameraStartPosition.y,
                cameraStartPosition.z) + cameraShakeAmount;
            Camera.transform.localPosition = newPosition;
        }

        private void OnFootstep() {
            if (CameraShakeDuration != 0f) {
                if (cameraShakeCoroutine != null) {
                    StopCoroutine(cameraShakeCoroutine);
                }

                cameraShakeCoroutine = ShakeCamera();
                StartCoroutine(cameraShakeCoroutine);
            }

            EventBus.Fire(Event.Footstep, new Evt(this));
        }

        private IEnumerator ShakeCamera() {
            float remainingTime = CameraShakeDuration;
            float amplitudeSign = 1f;

            while (remainingTime > 0f) {
                Vector3 amplitude = amplitudeSign * CameraShakeMaxAmount *
                    (remainingTime / CameraShakeDuration);
                cameraShakeAmount =
                    (Mathf.PerlinNoise(Time.time, 0f)) * amplitude;
                remainingTime -= Time.deltaTime;

                if (!shakeCameraInOneDirection) {
                    amplitudeSign *= -1f;
                }

                yield return null;
            }

            cameraShakeAmount = Vector3.zero;
        }
    }
}
