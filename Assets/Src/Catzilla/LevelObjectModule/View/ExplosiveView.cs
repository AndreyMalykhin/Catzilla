using UnityEngine;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;

namespace Catzilla.LevelObjectModule.View {
    public class ExplosiveView: MonoBehaviour, IPoolable {
        public struct ExplosionInfo {
            public readonly Vector3 Position;
            public readonly float Force;
            public readonly int HitObjectsCount;
            public readonly Collider[] HitObjects;

            public ExplosionInfo(Vector3 position, float force,
                int hitObjectsCount, Collider[] hitObjects) {
                Position = position;
                Force = force;
                HitObjectsCount = hitObjectsCount;
                HitObjects = hitObjects;
            }
        }

        public float ExplosionUpwardsModifier {
            get {return explosionUpwardsModifier;}
        }

        [Inject]
        private EventBus eventBus;

        [Inject]
        private PoolStorageView poolStorage;

        [SerializeField]
        private float explosionUpwardsModifier;

        [SerializeField]
        private float explosionRadius;

        [SerializeField]
        private float minForce;

        [SerializeField]
        private float maxForce;

        [SerializeField]
        private int maxHitObjects;

        [SerializeField]
        private LayerMask hitLayer;

        [SerializeField]
        private PoolableView poolable;

        private bool isExploded;
        private Collider[] hitObjects;

        public void Explode() {
            // DebugUtils.Log("ExplosiveView.Explode()");
            if (isExploded) {
                return;
            }

            Vector3 explosionSource = transform.position;
            float explosionForce = UnityEngine.Random.Range(minForce, maxForce);
            int hitObjectsCount = Mathf.Min(
                Physics.OverlapSphereNonAlloc(
                    explosionSource,
                    explosionRadius,
                    hitObjects,
                    hitLayer.value),
                maxHitObjects);
            var explosionInfo = new ExplosionInfo(explosionSource,
                explosionForce, hitObjectsCount, hitObjects);
            isExploded = true;
            eventBus.Fire((int) Events.ExplosiveExplode,
                new Evt(this, explosionInfo));
            poolStorage.ReturnLater(poolable);
        }

        void IPoolable.OnReturn() {
            isExploded = false;
        }

		void IPoolable.OnTake() {}

        private void OnDrawGizmos() {
            Gizmos.color = new Color(1f, 1f, 1f, 0.5f);
            Gizmos.DrawSphere(transform.position, explosionRadius);
        }

        private void Awake() {
            hitObjects = new Collider[maxHitObjects];
        }

        private void OnTriggerEnter(Collider collider) {
            eventBus.Fire((int) Events.ExplosiveTriggerEnter,
                new Evt(this, collider));
        }
    }
}
