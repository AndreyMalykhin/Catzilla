using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;

namespace Catzilla.LevelObjectModule.View {
    public class SmashedView: MonoBehaviour, IPoolable {
        private struct PartState {
            public Vector3 LocalPosition;
            public Quaternion LocalRotation;
        }

        public AudioClip SmashSound;
        public AudioSource AudioSource;

        [Inject]
        private PoolStorageView poolStorage;

        [SerializeField]
        private float lifetime;

        [SerializeField]
        private Transform[] parts;

        [SerializeField]
        private AlternatingView alternating;

        [SerializeField]
        private PoolableView poolable;

        private PartState[] initialPartStates;
        private float smashForce;
        private float smashUpwardsModifier;
        private Vector3 smashSourcePosition;

        [PostInject]
        public void OnConstruct() {
            // DebugUtils.Log("SmashedView.OnConstruct()");
            int partsCount = parts.Length;
            initialPartStates = new PartState[partsCount];

            for (int i = 0; i < partsCount; ++i) {
                Transform part = parts[i];
                initialPartStates[i] = new PartState{
                    LocalPosition = part.localPosition,
                    LocalRotation = part.localRotation
                };
            }
        }

        public void Init(SmashableView smashable, float smashForce,
            float smashUpwardsModifier, Vector3 smashSourcePosition) {
            // DebugUtils.Log("SmashedView.Init()");
            this.smashForce = smashForce;
            this.smashUpwardsModifier = smashUpwardsModifier;
            this.smashSourcePosition = smashSourcePosition;
            transform.position = smashable.transform.position;
            transform.rotation = smashable.transform.rotation;
            Transform[] smashableParts = smashable.Parts;

            for (int i = 0; i < smashableParts.Length; ++i) {
                Transform smashablePart = smashableParts[i];
                Transform smashedPart = parts[i];
                smashedPart.localPosition = smashablePart.localPosition;
                smashedPart.localRotation = smashablePart.localRotation;
            }

            if (alternating != null) {
                alternating.Material =
                    smashable.GetComponent<AlternatingView>().Material;
            }

            Smash();
        }

        void IPoolable.OnReturn() {
            for (int i = 0; i < parts.Length; ++i) {
                Transform part = parts[i];
                PartState initialPartState = initialPartStates[i];
                part.localPosition = initialPartState.LocalPosition;
                part.localRotation = initialPartState.LocalRotation;
                var partBody = part.GetComponent<Rigidbody>();

                if (partBody == null) {
                    continue;
                }

                partBody.Sleep();
            }
        }

		void IPoolable.OnTake() {}

        private void Smash() {
            // DebugUtils.Log("SmashedView.Smash()");
            float explosionRadius = 0f;

            for (int i = 0; i < parts.Length; ++i) {
                var partBody = parts[i].GetComponent<Rigidbody>();

                if (partBody == null) {
                    continue;
                }

                partBody.AddExplosionForce(smashForce, smashSourcePosition,
                    explosionRadius, smashUpwardsModifier);
            }

            Invoke("Dispose", lifetime);
        }

        private void Dispose() {
            poolStorage.ReturnLater(poolable);
        }
    }
}
