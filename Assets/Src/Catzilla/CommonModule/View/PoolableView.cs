using UnityEngine;
using UnityEngine.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using Zenject;
using Catzilla.CommonModule.Util;

namespace Catzilla.CommonModule.View {
    public class PoolableView: MonoBehaviour, IPoolable {
        public int PoolId {get {return poolId;}}
        public bool IsUI {get {return isUI;}}
        public bool DeactivateOnReturn {get {return deactivateOnReturn;}}
        public bool ActivateOnTake {get; set;}
        public bool IsInPool {get; set;}

        [SerializeField]
        private int poolId;

        [SerializeField]
        private bool isUI;

        [SerializeField]
        private bool deactivateOnReturn = true;

        private readonly List<IPoolable> poolables = new List<IPoolable>(8);

		void IPoolable.OnReturn() {
            for (var i = 0; i < poolables.Count; ++i) {
                poolables[i].OnReturn();
            }
        }

		void IPoolable.OnTake() {
            for (var i = 0; i < poolables.Count; ++i) {
                poolables[i].OnTake();
            }
        }

        private void Awake() {
            var components = GetComponents<MonoBehaviour>();

            for (var i = 0; i < components.Length; ++i) {
                var component = components[i];

                if (component is IPoolable && component != this) {
                    poolables.Add((IPoolable) component);
                }
            }
        }
    }
}
