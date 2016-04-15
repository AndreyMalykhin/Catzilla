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
        public delegate Attack AttackFilter(
            Attack attack, DamagingView source = null);

        public event ScoreFilter ScoreFilters {
            add {scoreFilters.Add(value);}
            remove {scoreFilters.Remove(value);}
        }

        public event AttackFilter AttackFilters {
            add {attackFilters.Add(value);}
            remove {attackFilters.Remove(value);}
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
                if (health == value) {
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
                frontSpeed = Mathf.Max(value, 0f);

                if (Animator.isInitialized) {
                    Animator.SetFloat(frontSpeedParam, frontSpeed);
                }
            }
        }

        public float SideSpeed {
            get {return sideSpeed;}
            set {sideSpeed = Mathf.Max(value, 0f);}
        }

        public int ActionsPerMinuteRank {
            get {return (int) (actionsCount / TotalLifetime * 6f);}
        }

        public float TotalLifetime {
            get {return Time.time - creationTime;}
        }

        public int DeathsCount {get {return deathsCount;}}
        public float RefuseChance {get; set;}
        public float RefuseDuration {get; set;}
        public float CriticalScoreChance {get; set;}
        public float DamageAbsorbChance {get; set;}

        public float DamageAbsorbFactor {
            get {return damageAbsorbFactor;}
            set {damageAbsorbFactor = Mathf.Clamp(value, 0f, 1f);}
        }

        public float CriticalScoreFactor {
            get {return criticalScoreFactor;}
            set {criticalScoreFactor = Mathf.Max(value, 0f);}
        }

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
        public int MaxHealth;
        public float BaseFrontSpeed;
        public float BaseSideSpeed;
        public float BaseRefuseChance;

        [Tooltip("In seconds")]
        public float BaseRefuseDuration;

        public float BaseCriticalScoreChance;
        public float BaseCriticalScoreFactor;
        public float BaseDamageAbsorbChance;
        public float BaseDamageAbsorbFactor;

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
        [Tooltip("In seconds")]
        private float refuseDelay;

        [SerializeField]
        [Tooltip("In seconds")]
        private float refuseCheckRate;

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
        private float sideSpeed;
        private int actionsCount;
        private float creationTime;
        private bool isRefusing;
        private int deathsCount;
        private int smashStreakLength;
        private int smashStreakScore;
        private float damageAbsorbFactor;
        private float criticalScoreFactor;
        private WaitForSeconds speedChangeDelayWaiter;
        private WaitForSeconds speedCheckRateWaiter;
        private readonly SmashStreak smashStreakBuffer = new SmashStreak();
        private readonly List<ScoreFilter> scoreFilters =
            new List<ScoreFilter>(4);
        private readonly List<AttackFilter> attackFilters =
            new List<AttackFilter>(4);

        [PostInject]
        public void OnConstruct() {
            // DebugUtils.Log("PlayerView.OnConstruct()");
            float halfWidth = Collider.bounds.extents.x;
            minX = levelMinX + halfWidth;
            maxX = levelMaxX - halfWidth;
            health = MaxHealth;
            cameraStartPosition = Camera.transform.localPosition;
            creationTime = Time.time;
            speedChangeDelayWaiter = new WaitForSeconds(speedChangeDelay);
            speedCheckRateWaiter = new WaitForSeconds(speedCheckRate);
            FrontSpeed = BaseFrontSpeed;
            SideSpeed = BaseSideSpeed;
            RefuseChance = BaseRefuseChance;
            RefuseDuration = BaseRefuseDuration;
            CriticalScoreChance = BaseCriticalScoreChance;
            CriticalScoreFactor = BaseCriticalScoreFactor;
            DamageAbsorbChance = BaseDamageAbsorbChance;
            DamageAbsorbFactor = BaseDamageAbsorbFactor;
            HUD = instantiator.InstantiatePrefabForComponent<HUDView>(
                hudProto.gameObject);
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

        public Attack FilterAttack(Attack attack, DamagingView source = null) {
            if (IsHealthFreezed || isDead) {
                return new Attack{Status = AttackStatus.Fail};
            }

            if (attack.Status != AttackStatus.Absorb
                && UnityEngine.Random.value <= DamageAbsorbChance
                && source != null) {
                attack.Damage -=
                    Mathf.RoundToInt(attack.Damage * damageAbsorbFactor);
                attack.Status = AttackStatus.Absorb;
            }

            for (int i = 0; i < attackFilters.Count; ++i) {
                attack = attackFilters[i](attack, source);
            }

            return attack;
        }

        public CriticalValue FilterScore(
            CriticalValue score, ScoreableView source = null) {
            if (IsScoreFreezed || isDead) {
                return new CriticalValue();
            }

            if (!score.IsCritical
                && UnityEngine.Random.value <= CriticalScoreChance
                && source != null
                && source.GetComponent<SmashableView>() != null) {
                score.IsCritical = true;
                score.Value =
                    Mathf.RoundToInt(score.Value * (1f + criticalScoreFactor));
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
            if (BaseRefuseChance != 0f && BaseRefuseDuration != 0f) {
                Invoke("EnsureRefuse", refuseDelay);
            }

            StartCoroutine(SpeedChanger());
            Animator.SetFloat(frontSpeedParam, frontSpeed);
        }

        private void OnDisable() {
            CancelInvoke("EnsureRefuse");
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
            ScoreableView scoreSource = null;
            extraScore = FilterScore(extraScore, scoreSource);

            if (extraScore > 0) {
                smashStreakBuffer.Length = smashStreakLength;
                smashStreakBuffer.ExtraScore = extraScore;
                eventBus.Fire((int) Events.PlayerSmashStreak,
                    new Evt(this, smashStreakBuffer));
                Score += extraScore;
            }

            ResetSmashStreak();
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

        private void EnsureRefuse() {
            // DebugUtils.Log("PlayerView.EnsureRefuse()");
            if (isRefusing) {
                isRefusing = false;
            } else {
                isRefusing = UnityEngine.Random.value <= RefuseChance;

                if (isRefusing) {
                    targetX = -targetX;
                    Invoke("EnsureRefuse", RefuseDuration);
                    return;
                }
            }

            Invoke("EnsureRefuse", refuseCheckRate);
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
            float sideSpeed = isRefusing ?
                Mathf.Min(this.sideSpeed, BaseSideSpeed) : this.sideSpeed;
            float newX = Mathf.MoveTowards(currentPosition.x, targetX,
                sideSpeed * Time.deltaTime);
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
