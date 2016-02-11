using UnityEngine;
using System;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;

namespace Catzilla.CommonModule.View {
    public class PoolableView
        : MonoBehaviour, IPoolable {
        public enum Event {Destroy}

        [Inject]
        public EventBus EventBus {get; set;}

        public int PoolId;
        public bool IsUI;

		public void Reset() {
            var components = GetComponents<MonoBehaviour>();

            for (var i = 0; i < components.Length; ++i) {
                var component = components[i];

                if (component != this && component is IPoolable) {
                    ((IPoolable) component).Reset();
                }
            }
        }

        private void OnDestroy() {
            // DebugUtils.Log("PoolableView.OnDestroy()");
            // can be null while app is destroying
            if (EventBus != null) {
                EventBus.Fire(Event.Destroy, new Evt(this));
            }
        }
    }
}
