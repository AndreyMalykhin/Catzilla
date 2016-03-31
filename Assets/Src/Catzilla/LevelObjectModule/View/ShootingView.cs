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

                if (shooter != null) {
                    StopCoroutine(shooter);
                }

                if (aimer != null) {
                    StopCoroutine(aimer);
                }

                if (target != null) {
                    targetOldPosition = target.transform.position;
                    aimee.LookAt(GetAimPoint());
                    shooter = Shooter();
                    aimer = Aimer();
                    StartCoroutine(shooter);
                    StartCoroutine(aimer);
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
        private float projectileMinSpeed;

        [SerializeField]
        private float projectileMaxSpeed;

        private Collider target;
        private int projectilePoolId;
        private float projectileSpeed;
        private WaitForSeconds shotPeriodWaiter;
        private WaitForSeconds burstPeriodWaiter;
        private WaitForSeconds shootingDelayWaiter;
        private WaitForSeconds aimPeriodWaiter;
        private IEnumerator shooter;
        private IEnumerator aimer;
        private Vector3 targetOldPosition;

        [PostInject]
        public void OnConstruct() {
            projectilePoolId =
                projectileProto.GetComponent<PoolableView>().PoolId;
            shotPeriodWaiter = new WaitForSeconds(shotPeriod);
            burstPeriodWaiter = new WaitForSeconds(burstPeriod);
            shootingDelayWaiter = new WaitForSeconds(shootingDelay);
            aimPeriodWaiter = new WaitForSeconds(aimPeriod);
            InitProjectileSpeed();
        }

        void IPoolable.OnReturn() {
            Target = null;
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

        private IEnumerator Aimer() {
            while (target != null) {
                Aim();
                yield return aimPeriodWaiter;
            }
        }

        private void Aim() {
            // DebugUtils.Log("ShootingView.Aim()");
            Quaternion rotationToTarget =
                Quaternion.LookRotation(GetAimPoint() - aimee.position);
            aimee.rotation = Quaternion.RotateTowards(
                aimee.rotation, rotationToTarget, aimSpeed * aimPeriod);
            targetOldPosition = target.transform.position;
        }

        private Vector3 GetAimPoint() {
            // DebugUtils.Log("ShootingView.GetAimPoint()");
            Vector3 targetPosition = target.transform.position;
            Vector3 targetSpeed =
                (targetPosition - targetOldPosition) / aimPeriod;
            Vector3 predictedOffset = targetSpeed * 1.5f;
            return new Vector3(targetPosition.x, aimee.position.y,
                targetPosition.z + predictedOffset.z);
        }

        private IEnumerator Shooter() {
            yield return shootingDelayWaiter;

            while (target != null) {
                for (int i = 0; i < shotsInBurst && target != null; ++i) {
                    Shoot();

                    if (shotPeriod != 0f) {
                        yield return shotPeriodWaiter;
                    }
                }

                if (burstPeriod != 0f) {
                    yield return burstPeriodWaiter;
                }
            }
        }

        private void Shoot() {
            // DebugUtils.Log("ShootingView.Shoot()");
            var projectile = poolStorage.Take(projectilePoolId)
                .GetComponent<ProjectileView>();
            projectile.transform.position = projectileSource.position;
            projectile.transform.rotation = projectileSource.rotation;
            projectile.Speed = projectileSpeed;
            eventBus.Fire((int) Events.ShootingShot, new Evt(this));
        }
    }
}
