using UnityEngine;
using System.Collections;

namespace Catzilla.LevelObjectModule.View {
    public class SmashedView: strange.extensions.mediation.impl.View {
        [SerializeField]
        private float explosionForce = 500f;

        [SerializeField]
        private float explosionUpwardsModifier = 0f;

        [SerializeField]
        private float pieceMinMass = 0.5f;

        [SerializeField]
        private float pieceMaxMass = 1f;

        [SerializeField]
        private float lifetime = 2f;

        private Vector3 sourcePosition;

        public void Init(Vector3 sourcePosition) {
            this.sourcePosition = sourcePosition;
        }

        protected override void Start() {
            base.Start();
            Explode();
            Destroy(gameObject, lifetime);
        }

        private void Explode() {
            float explosionRadius = 0f;
            var pieces = GetComponentsInChildren<Rigidbody>();

            for (int i = 0; i < pieces.Length; ++i) {
                pieces[i].mass = Random.Range(pieceMinMass, pieceMaxMass);
                pieces[i].AddExplosionForce(explosionForce, sourcePosition,
                    explosionRadius, explosionUpwardsModifier);
            }
        }
    }
}
