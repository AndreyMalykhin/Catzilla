using UnityEngine;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;

namespace Catzilla.LevelObjectModule.View {
    public class ExplosiveView: MonoBehaviour, IPoolable {
        public class ExplosionInfo {
            public Vector3 Position;
            public float Force;
            public int HitObjectsCount;
            public Collider[] HitObjects;
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
        private readonly ExplosionInfo explosionInfoBuffer =
            new ExplosionInfo();

        public void Explode() {
            // DebugUtils.Log("ExplosiveView.Explode()");
            if (isExploded) {
                return;
            }

            Vector3 explosionSource = transform.position;
            float explosionForce = UnityEngine.Random.Range(minForce, maxForce);
            int hitObjectsCount = 0;

            if (explosionRadius > 0f) {
                hitObjectsCount = Mathf.Min(
                    Physics.OverlapSphereNonAlloc(
                        explosionSource,
                        explosionRadius,
                        hitObjects,
                        hitLayer.value),
                    maxHitObjects);
            }

            explosionInfoBuffer.Position = explosionSource;
            explosionInfoBuffer.Force = explosionForce;
            explosionInfoBuffer.HitObjectsCount = hitObjectsCount;
            explosionInfoBuffer.HitObjects = hitObjects;
            isExploded = true;
            eventBus.Fire((int) Events.ExplosiveExplode,
                new Evt(this, explosionInfoBuffer));
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
