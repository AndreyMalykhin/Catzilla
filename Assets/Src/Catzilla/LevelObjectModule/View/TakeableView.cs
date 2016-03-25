using UnityEngine;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;

namespace Catzilla.LevelObjectModule.View {
    public class TakeableView: MonoBehaviour, IPoolable {
        public bool IsTaken {get {return isTaken;}}
        public AudioClip TakeSound;

        [Inject]
        private EventBus eventBus;

        [Inject]
        private PoolStorageView poolStorage;

        [SerializeField]
        private PoolableView poolable;

        private bool isTaken;

        public void Take() {
            isTaken = true;
            poolStorage.Return(poolable);
        }

        void IPoolable.Reset() {
            isTaken = false;
        }

        private void OnTriggerEnter(Collider collider) {
            eventBus.Fire((int) Events.TakeableTriggerEnter,
                new Evt(this, collider));
        }
    }
}
