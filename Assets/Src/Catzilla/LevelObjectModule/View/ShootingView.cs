﻿using UnityEngine;
using System.Collections;
using strange.extensions.context.api;
using strange.extensions.pool.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;

namespace Catzilla.LevelObjectModule.View {
    public class ShootingView
        : strange.extensions.mediation.impl.View, IPoolable {
        public enum Event {TriggerEnter}

        [Inject]
        public PoolStorageView PoolStorage {get; set;}

        [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
        public IEventDispatcher EventBus {get; set;}

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

        [SerializeField]
        private ProjectileView projectileProto;

        [SerializeField]
        private Transform projectileSource;

        [SerializeField]
        private float period = 5f;

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

		public void Retain() {Debug.Assert(false);}
		public void Release() {Debug.Assert(false);}

        private IEnumerator OnTriggerEnter(Collider collider) {
            yield return new WaitForFixedUpdate();
            EventBus.Dispatch(
                Event.TriggerEnter, new EventData(this, collider));
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
            yield return new WaitForSeconds(Random.Range(0f, period));

            while (true) {
                if (target == null) {
                    yield break;
                }

                Shoot();
                yield return new WaitForSeconds(period);
            }
        }

        private void Shoot() {
            // Debug.Log("ShootingView.Shoot()");
            var projectile = PoolStorage.Get(projectilePoolId);
            projectile.transform.position = projectileSource.position;
            projectile.transform.rotation = projectileSource.rotation;
        }
    }
}
