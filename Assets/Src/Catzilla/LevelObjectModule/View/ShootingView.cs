using UnityEngine;
using System.Collections;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;

namespace Catzilla.LevelObjectModule.View {
    public class ShootingView: strange.extensions.mediation.impl.View {
        public enum Event {Ready}

        [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
        public IEventDispatcher EventBus {get; set;}

        public Collider Target {get; set;}

        [SerializeField]
        private ProjectileView projectileProto;

        [SerializeField]
        private Transform projectileSource;

        [SerializeField]
        private float period = 3f;

        private Rigidbody body;

        [PostConstruct]
        public void OnReady() {
            body = GetComponent<Rigidbody>();
            EventBus.Dispatch(Event.Ready, this);
        }

        protected override void Start() {
            base.Start();
            StartCoroutine(StartAiming());
            StartCoroutine(StartShooting());
        }

        private IEnumerator StartAiming() {
            while (true) {
                yield return new WaitForFixedUpdate();

                if (Target == null) {
                    continue;
                }

                Aim();
            }
        }

        private void Aim() {
            Bounds targetBounds = Target.bounds;
            Vector3 shooterPosition = transform.position;
            var direction = new Vector3(
                targetBounds.center.x - shooterPosition.x,
                shooterPosition.y,
                targetBounds.center.z + targetBounds.extents.z - shooterPosition.z);
            body.MoveRotation(Quaternion.LookRotation(direction));
        }

        private IEnumerator StartShooting() {
            yield return new WaitForSeconds(Random.Range(0f, period));

            while (true) {
                if (Target != null) {
                    Shoot();
                }

                yield return new WaitForSeconds(period);
            }
        }

        private void Shoot() {
            // Debug.Log("ShootingView.Shoot()");
            Instantiate(projectileProto, projectileSource.position,
                projectileSource.rotation);
        }
    }
}
