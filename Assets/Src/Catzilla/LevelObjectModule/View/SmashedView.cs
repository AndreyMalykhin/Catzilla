using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;

namespace Catzilla.LevelObjectModule.View {
    public class SmashedView: MonoBehaviour, IPoolable {
        [Inject]
        public PoolStorageView PoolStorage {get; set;}

        public AudioClip SmashSound;
        public AudioSource AudioSource;

        private struct Piece {
            public Rigidbody Body;
            public Vector3 InitialPosition;
            public Quaternion InitialRotation;
        }

        [SerializeField]
        private float lifetime = 2f;

        [SerializeField]
        private bool overrideSmashParams = false;

        [SerializeField]
        private float overrideSmashUpwardsModifier = 0f;

        private PoolableView poolable;
        private Piece[] pieces;
        private float smashForce;
        private float smashUpwardsModifier;
        private Vector3 smashSourcePosition;
        private bool isSmashed;

        [PostInject]
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

        public void Init(float smashForce, float smashUpwardsModifier,
            Vector3 smashSourcePosition) {
            this.smashForce = smashForce;
            this.smashUpwardsModifier = smashUpwardsModifier;
            this.smashSourcePosition = smashSourcePosition;
        }

        public void Reset() {
            isSmashed = false;

            for (int i = 0; i < pieces.Length; ++i) {
                Piece piece = pieces[i];
                Rigidbody pieceBody = piece.Body;
                pieceBody.transform.localPosition = piece.InitialPosition;
                pieceBody.transform.localRotation = piece.InitialRotation;
                pieceBody.Sleep();
            }
        }

        private void FixedUpdate() {
            if (isSmashed) {
                return;
            }

            Smash();
        }

        private void Smash() {
            float explosionRadius = 0f;

            if (overrideSmashParams) {
                smashUpwardsModifier = overrideSmashUpwardsModifier;
            }

            for (int i = 0; i < pieces.Length; ++i) {
                Rigidbody pieceBody = pieces[i].Body;
                pieceBody.AddExplosionForce(smashForce, smashSourcePosition,
                    explosionRadius, smashUpwardsModifier);
            }

            Invoke("Dispose", lifetime);
            isSmashed = true;
        }

        private void Dispose() {
            PoolStorage.Return(poolable);
        }
    }
}
