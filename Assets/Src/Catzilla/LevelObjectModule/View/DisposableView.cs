using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;

namespace Catzilla.LevelObjectModule.View {
    public class DisposableView: MonoBehaviour {
        [Inject]
        public EventBus EventBus {get; set;}

        [Inject]
        public PoolStorageView PoolStorage {get; set;}

        [SerializeField]
        private PoolableView root;

        private PoolableView poolable;

        [PostInject]
        public void OnConstruct() {
            poolable = root == null ? GetComponent<PoolableView>() : root;
        }

        public void Dispose() {
            PoolStorage.Return(poolable);
        }

        private void OnTriggerExit(Collider collider) {
            EventBus.Fire((int) Events.DisposableTriggerExit,
                new Evt(this, collider));
        }
    }
}
