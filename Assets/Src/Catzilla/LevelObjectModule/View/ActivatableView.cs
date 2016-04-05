using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;

namespace Catzilla.LevelObjectModule.View {
    public class ActivatableView: MonoBehaviour, IPoolable {
        public bool IsActive {
            get {return isActive;}
            set {
                Profiler.BeginSample("ActivatableView.IsActive()");
                isActive = value;

                for (int i = 0; i < behaviours.Length; ++i) {
                    behaviours[i].enabled = value;
                }

                for (int i = 0; i < renderers.Length; ++i) {
                    renderers[i].enabled = value;
                }

                for (int i = 0; i < animators.Length; ++i) {
                    animators[i].enabled = value;
                }

                for (int i = 0; i < bodies.Length; ++i) {
                    if (!value) {
                        bodies[i].Sleep();
                    }
                }

                Profiler.EndSample();
            }
        }

        [Inject]
        private EventBus eventBus;

        [SerializeField]
        private MonoBehaviour[] behaviours;

        [SerializeField]
        private Renderer[] renderers;

        [SerializeField]
        private Animator[] animators;

        [SerializeField]
        private Rigidbody[] bodies;

        private bool isActive;

        [PostInject]
        public void OnConstruct() {
            IsActive = false;
        }

        void IPoolable.OnReturn() {
            IsActive = false;
        }

        void IPoolable.OnTake() {}

        private void OnTriggerEnter(Collider collider) {
            eventBus.Fire((int) Events.ActivatableTriggerEnter,
                new Evt(this, collider));
        }
    }
}
