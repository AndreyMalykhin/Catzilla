using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;

namespace Catzilla.LevelObjectModule.View {
    public class ShootingView: MonoBehaviour, IPoolable {
        public enum Event {TriggerEnter, Shot}

        public Collider Target {
            get {return target;}
            set {
                if (target == value) {
                    return;
                }

                if (target != null) {
                    if (shootingCoroutine != null) {
                        StopCoroutine(shootingCoroutine);
                    }
                }

                target = value;

                if (value != null) {
                    shootingCoroutine = StartShooting();
                    StartCoroutine(shootingCoroutine);
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
        private float aimSpeed = 360f;

        [Tooltip("In seconds")]
        [SerializeField]
        private float shotPeriod;

        [Tooltip("In seconds")]
        [SerializeField]
        private float burstPeriod = 8f;

        [SerializeField]
        private int shotsInBurst = 1;

        [Tooltip("In seconds")]
        [SerializeField]
        private float minDelay = 0.5f;

        [Tooltip("In seconds")]
        [SerializeField]
        private float maxDelay = 0.5f;

        [SerializeField]
        private float projectileMinSpeed = 8f;

        [SerializeField]
        private float projectileMaxSpeed = 16f;

        private Collider target;
        private IEnumerator shootingCoroutine;
        private int projectilePoolId;
        private float projectileSpeed;

        [PostInject]
        public void OnConstruct() {
            projectilePoolId =
                projectileProto.GetComponent<PoolableView>().PoolId;
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
            ViewUtils.DispatchNowOrAtFixedUpdate(this, GetEventBus,
                Event.TriggerEnter, new Evt(this, collider));
        }

        private EventBus GetEventBus() {
            return eventBus;
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

        private IEnumerator StartShooting() {
            yield return new WaitForSeconds(
                UnityEngine.Random.Range(minDelay, maxDelay));

            while (target != null) {
                yield return StartCoroutine(StartBurst());

                if (burstPeriod != 0f) {
                    yield return new WaitForSeconds(burstPeriod);
                }
            }
        }

        private IEnumerator StartBurst() {
            for (int i = 0; i < shotsInBurst && target != null; ++i) {
                Shoot();

                if (shotPeriod != 0f) {
                    yield return new WaitForSeconds(shotPeriod);
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
            eventBus.Fire(Event.Shot, new Evt(this));
        }
    }
}
