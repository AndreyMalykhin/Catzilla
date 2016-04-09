﻿using UnityEngine;
using UnityEngine.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using SmartLocalization;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;
using Catzilla.PlayerModule.View;
using Catzilla.LevelObjectModule.Model;

namespace Catzilla.LevelObjectModule.View {
    public class PlayerView: LevelObjectView {
        public class SmashStreak {
            public int Length;
            public int ExtraScore;
        }

        public delegate CriticalValue ScoreFilter(
            CriticalValue score, ScoreableView source = null);

        public event ScoreFilter OnScoreFilter {
            add {scoreFilters.Add(value);}
            remove {scoreFilters.Remove(value);}
        }

        public int ScoreBonusesTaken {get; set;}
        public int ResurrectionBonusesTaken {get; set;}
        public int SmashedCops {get; set;}

        public int Score {
            get {return score;}
            set {
                // DebugUtils.Log("PlayerView.Score()");
                if (score == value) {
                    return;
                }

                score = value;
                eventBus.Fire((int) Events.PlayerScoreChange, new Evt(this));
            }
        }

        public int Health {
            get {return health;}
            set {
                if (isDead || IsHealthFreezed || health == value) {
                    return;
                }

                int oldValue = health;
                health = value;
                health = Mathf.Clamp(health, 0, MaxHealth);
                eventBus.Fire(
                    (int) Events.PlayerHealthChange, new Evt(this, oldValue));

                if (health <= 0) {
                    Die();
                }
            }
        }

        public float FrontSpeed {
            get {return frontSpeed;}
            set {
                frontSpeed = value;

                if (Animator.isInitialized) {
                    Animator.SetFloat(frontSpeedParam, frontSpeed);
                }
            }
        }

        public int ActionsPerMinuteRank {
            get {return (int) (actionsCount / TotalLifetime * 6f);}
        }

        public float TotalLifetime {
            get {return Time.time - creationTime;}
        }

        public int DeathsCount {get {return deathsCount;}}

        public Collider Collider;
        public Camera Camera;
        public HUDView HUD;
        public Animator Animator;
        public AudioClip DeathSound;
        public AudioClip HurtSound;
        public AudioClip TreatSound;
        public AudioClip FootstepSound;
        public AudioClip SmashStreakSound;
        public AudioClip RefuseSound;
        public AudioSource LowPrioAudioSource;
        public AudioSource HighPrioAudioSource;
        public bool IsHealthFreezed;
        public bool IsScoreFreezed;
        public float SideSpeed;
        public int MaxHealth;

        private static readonly int frontSpeedParam =
            Animator.StringToHash("FrontSpeed");

        [Inject]
        private EventBus eventBus;

        [Inject("LevelMinX")]
        private float levelMinX;

        [Inject("LevelMaxX")]
        private float levelMaxX;

        [Inject]
        private IInstantiator instantiator;

        [SerializeField]
        private Rigidbody body;

        [SerializeField]
        private HUDView hudProto;

        [SerializeField]
        private LayerMask envLayer;

        [SerializeField]
        private ValueShakerView[] cameraShakers;

        [SerializeField]
        private float refuseChance;

        [SerializeField]
        [Tooltip("In seconds")]
        private float refuseDelay;

        [SerializeField]
        [Tooltip("In seconds")]
        private float refuseCheckRate;

        [SerializeField]
        [Tooltip("In seconds")]
        private float refuseDuration;

        [SerializeField]
        [Tooltip("In seconds")]
        private float speedChangeDelay;

        [SerializeField]
        [Tooltip("In seconds")]
        private float speedCheckRate;

        [SerializeField]
        private float speedChangeChance;

        [SerializeField]
        private float speedRestoreChance;

        [SerializeField]
        private float speedChangeAmount;

        [SerializeField]
        [Tooltip("In seconds")]
        private float smashStreakDelay;

        [SerializeField]
        private int smashStreakMinLength;

        [SerializeField]
        private float smashStreakScoreLengthFactor;

        private Vector3 cameraStartPosition;
        private int score;
        private int health;
        private bool isDead;
        private float minX;
        private float maxX;
        private float targetX;
        private float frontSpeed;
        private int actionsCount;
        private float creationTime;
        private bool isRefusing;
        private int deathsCount;
        private int smashStreakLength;
        private int smashStreakScore;
        private WaitForSeconds refuseCheckRateWaiter;
        private WaitForSeconds refuseDurationWaiter;
        private WaitForSeconds speedChangeDelayWaiter;
        private WaitForSeconds speedCheckRateWaiter;
        private WaitForSeconds refuseDelayWaiter;
        private readonly SmashStreak smashStreakBuffer = new SmashStreak();
        private readonly List<ScoreFilter> scoreFilters =
            new List<ScoreFilter>(4);

        [PostInject]
        public void OnConstruct() {
            // DebugUtils.Log("PlayerView.OnConstruct()");
            float halfWidth = Collider.bounds.extents.x;
            minX = levelMinX + halfWidth;
            maxX = levelMaxX - halfWidth;
            health = MaxHealth;
            cameraStartPosition = Camera.transform.localPosition;
            creationTime = Time.time;
            refuseCheckRateWaiter = new WaitForSeconds(refuseCheckRate);
            refuseDurationWaiter = new WaitForSeconds(refuseDuration);
            speedChangeDelayWaiter = new WaitForSeconds(speedChangeDelay);
            speedCheckRateWaiter = new WaitForSeconds(speedCheckRate);
            refuseDelayWaiter = new WaitForSeconds(refuseDelay);
            HUD = instantiator.InstantiatePrefab(hudProto.gameObject)
                .GetComponent<HUDView>();
            eventBus.Fire((int) Events.PlayerConstruct, new Evt(this));
        }

        public void AddSmash(int score) {
            ++smashStreakLength;
            smashStreakScore += score;
            CancelInvoke("EnsureSmashStreak");
            Invoke("EnsureSmashStreak", smashStreakDelay);
        }

        public void Resurrect() {
            if (!isDead) {
                return;
            }

            isDead = false;
            targetX = body.position.x;
            Animator.enabled = true;
            Health = MaxHealth;
            eventBus.Fire((int) Events.PlayerResurrect, new Evt(this));
        }

        public CriticalValue FilterScore(
            CriticalValue score, ScoreableView source) {
            if (IsScoreFreezed) {
                return 0;
            }

            for (int i = 0; i < scoreFilters.Count; ++i) {
                score = scoreFilters[i](score, source);
            }

            return score;
        }

        public void ShakeCamera(
            Vector3 amount, float duration, bool inOneDirection) {
            // DebugUtils.Log("PlayerView.ShakeCamera()");
            if (duration == 0f || amount == Vector3.zero) {
                return;
            }

            for (int i = 0; i < cameraShakers.Length; ++i) {
                ValueShakerView shaker = cameraShakers[i];

                if (shaker.IsShaking) {
                    continue;
                }

                shaker.Shake(amount, duration, inOneDirection);
                break;
            }
        }

        private void OnEnable() {
            StartCoroutine(Refuser());
            StartCoroutine(SpeedChanger());
            Animator.SetFloat(frontSpeedParam, frontSpeed);
        }

        private void EnsureSmashStreak() {
            if (isDead
                || smashStreakLength < smashStreakMinLength
                || smashStreakScore <= 0) {
                ResetSmashStreak();
                return;
            }

            int extraScore = (int) (smashStreakScore *
                smashStreakLength * smashStreakScoreLengthFactor);
            smashStreakBuffer.Length = smashStreakLength;
            smashStreakBuffer.ExtraScore = extraScore;
            ResetSmashStreak();
            eventBus.Fire((int) Events.PlayerSmashStreak,
                new Evt(this, smashStreakBuffer));
            ScoreableView scoreSource = null;
            Score += FilterScore(extraScore, scoreSource);
        }

        private void ResetSmashStreak() {
            smashStreakLength = 0;
            smashStreakScore = 0;
        }

        private void OnDestroy() {
            if (HUD != null) {
                Destroy(HUD.gameObject);
            }

            eventBus.Fire((int) Events.PlayerDestroy, new Evt(this));
        }

        private void Die() {
            if (isDead) {
                return;
            }

            isDead = true;
            Animator.enabled = false;
            ++deathsCount;
            eventBus.Fire((int) Events.PlayerDeath, new Evt(this));
        }

        private void Update() {
            if (isDead) {
                return;
            }

            if (Input.GetMouseButtonDown(0)) {
                ++actionsCount;

                if (isRefusing) {
                    eventBus.Fire((int) Events.PlayerRefuse, new Evt(this));
                } else {
                    SetTargetX(Input.mousePosition);
                }
            }
        }

        private IEnumerator Refuser() {
            // DebugUtils.Log("PlayerView.Refuser()");
            if (refuseChance == 0f || refuseDuration == 0f) {
                yield break;
            }

            yield return refuseDelayWaiter;

            while (true) {
                isRefusing = UnityEngine.Random.value <= refuseChance;

                if (isRefusing) {
                    yield return refuseDurationWaiter;
                    isRefusing = false;
                }

                yield return refuseCheckRateWaiter;
            }
        }

        private IEnumerator SpeedChanger() {
            if (speedChangeChance == 0f) {
                yield break;
            }

            yield return speedChangeDelayWaiter;

            while (true) {
                if (UnityEngine.Random.value <= speedChangeChance) {
                    float normalSpeed = frontSpeed;
                    FrontSpeed += UnityEngine.Random.Range(0, 2) == 0 ?
                        speedChangeAmount : -speedChangeAmount;

                    while (true) {
                        if (UnityEngine.Random.value <= speedRestoreChance) {
                            FrontSpeed = normalSpeed;
                            break;
                        }

                        yield return speedCheckRateWaiter;
                    }
                }

                yield return speedCheckRateWaiter;
            }
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

        private void LateUpdate() {
            MoveCamera();
        }

        private void MoveCamera() {
            // DebugUtils.Log("PlayerView.MoveCamera()");
            var newPosition = new Vector3(
                cameraStartPosition.x - transform.position.x,
                cameraStartPosition.y,
                cameraStartPosition.z);

            for (int i = 0; i < cameraShakers.Length; ++i) {
                newPosition += cameraShakers[i].Amount;
            }

            Camera.transform.localPosition = newPosition;
        }

        private void FixedUpdate() {
            if (isDead) {
                return;
            }

            Move();
        }

        private void Move() {
            Vector3 currentPosition = body.position;
            float newX = Mathf.MoveTowards(
                currentPosition.x, targetX, SideSpeed * Time.deltaTime);
            float newZ = currentPosition.z + frontSpeed * Time.deltaTime;
            body.MovePosition(new Vector3(newX, currentPosition.y, newZ));
        }

        private void Start() {
            targetX = body.position.x;
        }

        private void OnTriggerEnter(Collider collider) {
            eventBus.Fire((int) Events.PlayerTriggerEnter,
                new Evt(this, collider));
        }

        private void OnFootstep() {
            eventBus.Fire((int) Events.PlayerFootstep, new Evt(this));
        }
    }
}
