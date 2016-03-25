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

                if (target != null) {
                    if (shooter != null) {
                        StopCoroutine(shooter);
                    }
                }

                target = value;

                if (value != null) {
                    shooter = Shooter();
                    StartCoroutine(shooter);
                    aimee.LookAt(GetAimPoint());
                }
             }
        }

        public AudioClip ShotSound;
        public AudioSource AudioSource;

        [Inject]
        private PoolStorageView poolStorage;

        [Inject]
        private EventBus eventBus;

        [SerializeField]
        private ProjectileView projectileProto;

        [SerializeField]
        private Transform projectileSource;

        [SerializeField]
        private Transform aimee;

        [Tooltip("In degrees")]
        [SerializeField]
        private float aimSpeed;

        [SerializeField]
        [FormerlySerializedAs("targetCheckPeriod")]
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
        private IEnumerator shooter;

        [PostInject]
        public void OnConstruct() {
            projectilePoolId =
                projectileProto.GetComponent<PoolableView>().PoolId;
            shotPeriodWaiter = new WaitForSeconds(shotPeriod);
            burstPeriodWaiter = new WaitForSeconds(burstPeriod);
            shootingDelayWaiter = new WaitForSeconds(shootingDelay);
            SetRandomProjectileSpeed();
        }

        void IPoolable.Reset() {
            Target = null;
            SetRandomProjectileSpeed();
        }

        private void SetRandomProjectileSpeed() {
            projectileSpeed =
                Random.Range(projectileMinSpeed, projectileMaxSpeed);
        }

        private void OnTriggerEnter(Collider collider) {
            eventBus.Fire((int) Events.ShootingTriggerEnter,
                new Evt(this, collider));
        }

        private void FixedUpdate() {
            if (target == null) {
                return;
            }

            Aim();
        }

        private void Aim() {
            Quaternion rotationToTarget =
                Quaternion.LookRotation(GetAimPoint() - aimee.position);
            aimee.rotation = Quaternion.RotateTowards(
                aimee.rotation, rotationToTarget, aimSpeed * Time.deltaTime);
        }

        private Vector3 GetAimPoint() {
            Bounds targetBounds = target.bounds;
            return new Vector3(
                targetBounds.center.x,
                aimee.position.y,
                targetBounds.max.z);
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
