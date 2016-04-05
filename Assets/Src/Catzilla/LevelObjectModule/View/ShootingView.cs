using UnityEngine;
using UnityEngine.Serialization;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;

namespace Catzilla.LevelObjectModule.View {
    public class ShootingView: MonoBehaviour, IPoolable {
        public Collider Target {
            get {return target;}
            set {
                if (target == value) {
                    return;
                }

                target = value;
                CancelInvoke("Aim");
                CancelInvoke("ShootBurst");
                CancelInvoke("ShootSingle");

                if (target == null) {
                    return;
                }

                targetPrevPosition = target.transform.position;
                prevAimTime = Time.time;
                targetPrevSpeed = Vector3.zero;
                aimee.LookAt(new Vector3(targetPrevPosition.x,
                    aimee.position.y, targetPrevPosition.z));
                InvokeRepeating("Aim", aimPeriod, aimPeriod);

                if (shotsLeft > 0) {
                    InvokeRepeating("ShootBurst", shootingDelay, burstPeriod);
                }
             }
        }

        public AudioClip ShotSound;
        public AudioSource AudioSource;

        [Inject]
        private PoolStorageView poolStorage;

        [Inject]
        private EventBus eventBus;

        [Inject("LevelMinX")]
        private float levelMinX;

        [Inject("LevelMaxX")]
        private float levelMaxX;

        [SerializeField]
        private ProjectileView projectileProto;

        [SerializeField]
        private Transform projectileSource;

        [SerializeField]
        private Transform aimee;

        [Tooltip("In degrees")]
        [SerializeField]
        private float aimSpeed;

        [Tooltip("In seconds")]
        [SerializeField]
        private float aimPeriod;

        [SerializeField]
        [Tooltip("In seconds")]
        private float shootingDelay;

        [Tooltip("In seconds")]
        [SerializeField]
        private float shotPeriod;

        [Tooltip("In seconds")]
        [SerializeField]
        private float burstPeriod;

        [SerializeField]
        private int shotsInBurst;

        [SerializeField]
        private int shotsCount;

        [SerializeField]
        private float projectileMinSpeed;

        [SerializeField]
        private float projectileMaxSpeed;

        [SerializeField]
        private float shootAheadTargetSpeedFactor;

        private Collider target;
        private int projectilePoolId;
        private float projectileSpeed;
        private Vector3 targetPrevPosition;
        private Vector3 targetPrevSpeed;
        private float prevAimTime;
        private int shotsLeft;

        [PostInject]
        public void OnConstruct() {
            DebugUtils.Assert(aimPeriod > 0f);
            DebugUtils.Assert(shootingDelay > aimPeriod);
            projectilePoolId =
                projectileProto.GetComponent<PoolableView>().PoolId;
            shotsLeft = shotsCount;
            InitProjectileSpeed();
        }

        void IPoolable.OnReturn() {
            Target = null;
            prevAimTime = 0f;
            shotsLeft = shotsCount;
            targetPrevPosition = Vector3.zero;
            targetPrevSpeed = Vector3.zero;
            InitProjectileSpeed();
        }

		void IPoolable.OnTake() {}

        private void InitProjectileSpeed() {
            projectileSpeed =
                Random.Range(projectileMinSpeed, projectileMaxSpeed);
        }

        private void OnTriggerEnter(Collider collider) {
            eventBus.Fire((int) Events.ShootingTriggerEnter,
                new Evt(this, collider));
        }

        private void Aim() {
            // DebugUtils.Log("ShootingView.Aim()");
            if (target == null) {
                return;
            }

            Vector3 targetPosition = target.transform.position;
            Vector3 targetSpeed = (targetPosition - targetPrevPosition) /
                (Time.time - prevAimTime);
            Vector3 predictedOffset = new Vector3();

            if (shotsLeft > 0) {
                predictedOffset = Vector3.Max(targetSpeed, targetPrevSpeed)
                    * shootAheadTargetSpeedFactor;
            }

            Vector3 aimPoint = new Vector3(targetPosition.x, aimee.position.y,
                targetPosition.z + predictedOffset.z);
            Quaternion rotationToTarget =
                Quaternion.LookRotation(aimPoint - aimee.position);
            aimee.rotation = Quaternion.RotateTowards(
                aimee.rotation, rotationToTarget, aimSpeed * aimPeriod);
            targetPrevPosition = target.transform.position;
            targetPrevSpeed = targetSpeed;
            prevAimTime = Time.time;
        }

        private void ShootBurst() {
            if (target == null) {
                return;
            }

            for (int i = 0; i < shotsInBurst; ++i) {
                Invoke("ShootSingle", shotPeriod * i);
            }
        }

        private void ShootSingle() {
            // DebugUtils.Log("ShootingView.Shoot()");
            if (target == null) {
                return;
            }

            var projectile = poolStorage.Take(projectilePoolId)
                .GetComponent<ProjectileView>();
            projectile.transform.position = projectileSource.position;
            projectile.transform.rotation = projectileSource.rotation;
            projectile.Speed = projectileSpeed;

            if (--shotsLeft <= 0) {
                CancelInvoke("ShootBurst");
                CancelInvoke("ShootSingle");
            }

            eventBus.Fire((int) Events.ShootingShot, new Evt(this));
        }
    }
}
