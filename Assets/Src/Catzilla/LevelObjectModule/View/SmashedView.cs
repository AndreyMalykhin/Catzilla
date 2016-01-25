using UnityEngine;
using System.Collections;
using strange.extensions.pool.api;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;

namespace Catzilla.LevelObjectModule.View {
    public class SmashedView
        : strange.extensions.mediation.impl.View, IPoolable {
        [Inject]
        public PoolStorageView PoolStorage {get; set;}

		public bool retain {get {return false;}}
        public AudioClip SmashSound;
        public AudioSource AudioSource;

        private struct Piece {
            public Rigidbody Body;
            public Vector3 InitialPosition;
            public Quaternion InitialRotation;
        }

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

        private PoolableView poolable;
        private Piece[] pieces;

        [PostConstruct]
        public void OnConstruct() {
            poolable = GetComponent<PoolableView>();
            var pieceBodies = GetComponentsInChildren<Rigidbody>();
            pieces = new Piece[pieceBodies.Length];

            for (int i = 0; i < pieceBodies.Length; ++i) {
                var pieceBody = pieceBodies[i];
                Piece piece = new Piece{
                    Body = pieceBody,
                    InitialPosition = pieceBody.transform.localPosition,
                    InitialRotation = pieceBody.transform.localRotation
                };
                pieces[i] = piece;
            }
        }

        public void Init(Vector3 sourcePosition) {
            StartCoroutine(Explode(sourcePosition));
        }

        public void Restore() {
            for (int i = 0; i < pieces.Length; ++i) {
                Piece piece = pieces[i];
                Rigidbody pieceBody = piece.Body;
                pieceBody.transform.localPosition = piece.InitialPosition;
                pieceBody.transform.localRotation = piece.InitialRotation;
                pieceBody.Sleep();
            }
        }

		public void Retain() {Debug.Assert(false);}
		public void Release() {Debug.Assert(false);}

        private IEnumerator Explode(Vector3 sourcePosition) {
            yield return new WaitForFixedUpdate();
            float explosionRadius = 0f;

            for (int i = 0; i < pieces.Length; ++i) {
                Rigidbody pieceBody = pieces[i].Body;
                pieceBody.mass = Random.Range(pieceMinMass, pieceMaxMass);
                pieceBody.AddExplosionForce(explosionForce, sourcePosition,
                    explosionRadius, explosionUpwardsModifier);
            }

            Invoke("Dispose", lifetime);
        }

        private void Dispose() {
            PoolStorage.Return(poolable);
        }
    }
}
