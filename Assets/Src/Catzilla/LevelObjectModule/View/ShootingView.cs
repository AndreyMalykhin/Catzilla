using UnityEngine;
using System.Collections;
using strange.extensions.context.api;
using strange.extensions.pool.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;

namespace Catzilla.LevelObjectModule.View {
    public class ShootingView
        : strange.extensions.mediation.impl.View, IPoolable {
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
                    StopCoroutine(aimingCoroutine);
                    StopCoroutine(shootingCoroutine);
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

		public bool retain {get {return false;}}
        public AudioClip ShotSound;
        public AudioSource AudioSource;

        [SerializeField]
        private ProjectileView projectileProto;

        [SerializeField]
        private Transform projectileSource;

        [SerializeField]
        private float period = 5f;

        [SerializeField]
        private float maxDelay = 2.5f;

        private Rigidbody body;
        private Collider target;
        private IEnumerator aimingCoroutine;
        private IEnumerator shootingCoroutine;
        private int projectilePoolId;

        [PostConstruct]
        public void OnConstruct() {
            body = GetComponent<Rigidbody>();
            projectilePoolId =
                projectileProto.GetComponent<PoolableView>().PoolId;
        }

        public void Restore() {
            Target = null;
        }

		public void Retain() {DebugUtils.Assert(false);}
		public void Release() {DebugUtils.Assert(false);}

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
            var projectile = PoolStorage.Get(projectilePoolId);
            projectile.transform.position = projectileSource.position;
            projectile.transform.rotation = projectileSource.rotation;
            EventBus.Fire(Event.Shot, new Evt(this));
        }
    }
}
