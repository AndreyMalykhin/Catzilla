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

                if (target != null) {
                    targetOldPosition = target.transform.position;
                    aimee.LookAt(GetAimPoint());
                    InvokeRepeating("Aim", aimPeriod, aimPeriod);
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
        private float projectileMinSpeed;

        [SerializeField]
        private float projectileMaxSpeed;

        private Collider target;
        private int projectilePoolId;
        private float projectileSpeed;
        private Vector3 targetOldPosition;

        [PostInject]
        public void OnConstruct() {
            projectilePoolId =
                projectileProto.GetComponent<PoolableView>().PoolId;
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

        private void Aim() {
            // DebugUtils.Log("ShootingView.Aim()");
            if (target == null) {
                return;
            }

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
            eventBus.Fire((int) Events.ShootingShot, new Evt(this));
        }
    }
}
