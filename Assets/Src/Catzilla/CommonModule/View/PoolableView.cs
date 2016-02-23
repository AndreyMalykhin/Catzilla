using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Zenject;
using Catzilla.CommonModule.Util;

namespace Catzilla.CommonModule.View {
    public class PoolableView: MonoBehaviour, IPoolable {
        public int PoolId;
        public bool IsUI;

        private readonly List<IPoolable> poolables = new List<IPoolable>(4);

        [PostInject]
        public void OnConstruct() {
            var components = GetComponents<MonoBehaviour>();

            for (var i = 0; i < components.Length; ++i) {
                var component = components[i];

                if (component is IPoolable && component != this) {
                    poolables.Add((IPoolable) component);
                }
            }
        }

		void IPoolable.Reset() {
            for (var i = 0; i < poolables.Count; ++i) {
                poolables[i].Reset();
            }
        }
    }
}
