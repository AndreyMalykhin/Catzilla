using UnityEngine;
using Zenject;
using Catzilla.CommonModule.Util;

namespace Catzilla.LevelObjectModule.View {
    public class ExplosiveView: MonoBehaviour {
        public enum Event {Explode}

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

        [Inject]
        private EventBus eventBus;

        [SerializeField]
        private SmashableView smashable;

        [SerializeField]
        private float explosionUpwardsModifier;

        [SerializeField]
        private float explosionRadius;

        [SerializeField]
        private float minForce = 256f;

        [SerializeField]
        private float maxForce = 512f;

        [SerializeField]
        private int maxHitObjects;

        [SerializeField]
        private LayerMask hitLayer;

        private Collider[] hitObjects;

        public void Explode() {
            // DebugUtils.Log("ExplosiveView.Explode()");
            Vector3 explosionSource = transform.position;
            float explosionForce = UnityEngine.Random.Range(minForce, maxForce);
            int hitObjectsCount = Mathf.Min(
                Physics.OverlapSphereNonAlloc(
                    explosionSource,
                    explosionRadius,
                    hitObjects,
                    hitLayer.value),
                maxHitObjects);
            smashable.Smash(
                explosionForce, explosionUpwardsModifier, explosionSource);
            var explosionInfo = new ExplosionInfo(explosionSource,
                explosionForce, hitObjectsCount, hitObjects);
            eventBus.Fire(Event.Explode, new Evt(this, explosionInfo));
        }

        private void OnDrawGizmos() {
            Gizmos.color = new Color(1f, 1f, 1f, 0.5f);
            Gizmos.DrawSphere(transform.position, explosionRadius);
        }

        private void Awake() {
            hitObjects = new Collider[maxHitObjects];
        }
    }
}
