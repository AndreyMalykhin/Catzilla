﻿using UnityEngine;
using System.Collections;
using SmartLocalization;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
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
            Footstep
        }

        [Inject]
        public EventBus EventBus {get; set;}

        [Inject("LevelMinX")]
        public float LevelMinX {get; set;}

        [Inject("LevelMaxX")]
        public float LevelMaxX {get; set;}

        [Inject("EnvLayer")]
        public int EnvLayer {get; set;}

        public Collider Collider;
        public Camera Camera;
        public PlayerHUDView HUDProto;
        public Animator Animator;
        public AudioClip DeathSound;
        public AudioClip ScoreSound;
        public AudioClip FootstepSound;
        public AudioSource AudioSource;
        public bool IsHealthFreezed;
        public bool IsScoreFreezed;
        public float MinSmashForce = 75f;
        public float MaxSmashForce = 125f;
        public float SmashUpwardsModifier = -5f;
        public float FrontSpeed = 5f;
        public float SideSpeed = 5f;
        public int MaxHealth = 100;
        public float CameraShakeDuration = 0.5f;

        public int Score {
            get {
                return score;
            }
            set {
                // DebugUtils.Log("PlayerView.Score set");

                if (IsScoreFreezed) {
                    return;
                }

                score = value;
                EventBus.Fire(Event.ScoreChange, new Evt(this));
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

                int oldValue = health;
                health = value;
                health = Mathf.Clamp(health, 0, MaxHealth);
                EventBus.Fire(
                    Event.HealthChange, new Evt(this, oldValue));

                if (health > 0) {
                    return;
                }

                Die();
            }
        }

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
        private PlayerHUDView HUD;

        [PostConstruct]
        public void OnConstruct() {
            // DebugUtils.Log("PlayerView.OnConstruct()");
            body = GetComponent<Rigidbody>();
            targetX = body.position.x;
            float halfWidth = Collider.bounds.extents.x;
            minX = LevelMinX + halfWidth;
            maxX = LevelMaxX - halfWidth;
            health = MaxHealth;
            cameraStartPosition = Camera.transform.localPosition;
            HUD = (PlayerHUDView) Instantiate(HUDProto);
            EventBus.Fire(Event.Construct, new Evt(this));
        }

        public void Resurrect() {
            if (!isDead) {
                return;
            }

            isDead = false;
            targetX = body.position.x;
            Health = MaxHealth;
            Animator.enabled = true;
            EventBus.Fire(Event.Resurrect, new Evt(this));
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

        private void SetTargetX(Vector3 mousePosition) {
            Ray ray = Camera.ScreenPointToRay(mousePosition);
            RaycastHit hit;

            if (!Physics.Raycast(
                    ray, out hit, Camera.farClipPlane, EnvLayer)) {
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
                cameraStartPosition.z) + Camera.transform.up *
                cameraShakeAmount.y;
            Camera.transform.localPosition = newPosition;
        }

        private void OnFootstep() {
            if (cameraShakeCoroutine != null) {
                StopCoroutine(cameraShakeCoroutine);
            }

            cameraShakeCoroutine = ShakeCamera();
            StartCoroutine(cameraShakeCoroutine);
            EventBus.Fire(Event.Footstep, new Evt(this));
        }

        private IEnumerator ShakeCamera() {
            float remainingTime = CameraShakeDuration;

            while (remainingTime > 0f) {
                float amplitude = -1f * remainingTime / CameraShakeDuration;
                cameraShakeAmount = new Vector3(
                    0f, Mathf.PerlinNoise(Time.time, 0f) * amplitude, 0f);
                remainingTime -= Time.deltaTime;
                yield return null;
            }

            cameraShakeAmount = Vector3.zero;
        }
    }
}
