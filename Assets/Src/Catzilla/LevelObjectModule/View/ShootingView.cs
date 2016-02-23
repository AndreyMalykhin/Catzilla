using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;

namespace Catzilla.LevelObjectModule.View {
    public class ShootingView: MonoBehaviour, IPoolable {
        public enum Event {TriggerEnter, Shot}

        [Inject]
        public PoolStorageView PoolStorage {get; set;}

        [Inject]
        public EventBus EventBus {get; set;}

        public Collider Target {
            get {
                return target;
            }
            set {
                if (target == value) {
                    return;
                }

                if (target != null) {
                    if (aimingCoroutine != null) {
                        StopCoroutine(aimingCoroutine);
                    }

                    if (shootingCoroutine != null) {
                        StopCoroutine(shootingCoroutine);
                    }
                }

                target = value;

                if (value != null) {
                    aimingCoroutine = StartAiming();
                    shootingCoroutine = StartShooting();
                    StartCoroutine(aimingCoroutine);
                    StartCoroutine(shootingCoroutine);
                }
            }
        }

        public AudioClip ShotSound;
        public AudioSource AudioSource;

        [SerializeField]
        private ProjectileView projectileProto;

        [SerializeField]
        private Transform projectileSource;

        [Tooltip("In seconds")]
        [SerializeField]
        private float period = 5f;

        [Tooltip("In seconds")]
        [SerializeField]
        private float maxDelay = 2.5f;

        private Rigidbody body;
        private Collider target;
        private IEnumerator aimingCoroutine;
        private IEnumerator shootingCoroutine;
        private int projectilePoolId;

        [PostInject]
        public void OnConstruct() {
            body = GetComponent<Rigidbody>();
            projectilePoolId =
                projectileProto.GetComponent<PoolableView>().PoolId;
        }

        void IPoolable.Reset() {
            Target = null;
        }

        private void OnTriggerEnter(Collider collider) {
            ViewUtils.DispatchNowOrAtFixedUpdate(this, GetEventBus,
                Event.TriggerEnter, new Evt(this, collider));
        }

        private EventBus GetEventBus() {
            return EventBus;
        }

        private IEnumerator StartAiming() {
            while (true) {
                yield return new WaitForFixedUpdate();

                if (target == null) {
                    yield break;
                }

                Aim();
            }
        }

        private void Aim() {
            Bounds targetBounds = target.bounds;
            Vector3 shooterPosition = transform.position;
            var direction = new Vector3(
                targetBounds.center.x - shooterPosition.x,
                shooterPosition.y,
                targetBounds.center.z + targetBounds.extents.z - shooterPosition.z);
            body.MoveRotation(Quaternion.LookRotation(direction));
        }

        private IEnumerator StartShooting() {
            yield return new WaitForSeconds(0.5f + Random.Range(0f, maxDelay));

            while (true) {
                if (target == null) {
                    yield break;
                }

                Shoot();
                yield return new WaitForSeconds(period);
            }
        }

        private void Shoot() {
            // DebugUtils.Log("ShootingView.Shoot()");
            var projectile = PoolStorage.Take(projectilePoolId);
            projectile.transform.position = projectileSource.position;
            projectile.transform.rotation = projectileSource.rotation;
            EventBus.Fire(Event.Shot, new Evt(this));
        }
    }
}
