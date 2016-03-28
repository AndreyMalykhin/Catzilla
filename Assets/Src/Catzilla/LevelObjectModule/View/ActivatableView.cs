using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;

namespace Catzilla.LevelObjectModule.View {
    public class ActivatableView: MonoBehaviour, IPoolable {
        public bool IsActive {
            get {return isActive;}
            set {
                isActive = value;

                for (int i = 0; i < behaviours.Length; ++i) {
                    behaviours[i].enabled = value;
                }

                for (int i = 0; i < renderers.Length; ++i) {
                    renderers[i].enabled = value;
                }
            }
        }

        [Inject]
        private EventBus eventBus;

        [SerializeField]
        private MonoBehaviour[] behaviours;

        [SerializeField]
        private Renderer[] renderers;

        private bool isActive;

        [PostInject]
        public void OnConstruct() {
            IsActive = false;
            eventBus.Fire((int) Events.ActivatableConstruct, new Evt(this));
        }

        void IPoolable.OnReturn() {
            IsActive = false;
        }

        void IPoolable.OnTake() {
            eventBus.Fire((int) Events.ActivatableConstruct, new Evt(this));
        }

        private void OnTriggerEnter(Collider collider) {
            eventBus.Fire((int) Events.ActivatableTriggerEnter,
                new Evt(this, collider));
        }
    }
}
