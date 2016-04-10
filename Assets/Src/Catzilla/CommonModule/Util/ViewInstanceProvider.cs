using UnityEngine;
using System;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.View;

namespace Catzilla.CommonModule.Util {
    public class ViewInstanceProvider: Pool<PoolableView>.IInstanceProvider {
        private readonly PoolableView proto;
        private readonly Transform parent;
        private readonly IInstantiator instantiator;

        public ViewInstanceProvider(PoolableView proto,
            IInstantiator instantiator, Transform parent = null) {
            this.proto = proto;
            this.parent = parent;
            this.instantiator = instantiator;
        }

        public PoolableView Get() {
            var instance =
                instantiator.InstantiatePrefabForComponent<PoolableView>(
                    proto.gameObject);

            if (instance.DeactivateOnReturn) {
                GameObject instanceObject = instance.gameObject;
                instance.ActivateOnTake = instanceObject.activeSelf;
                instanceObject.SetActive(false);
            }

            if (parent != null) {
                instance.transform.SetParent(parent, !instance.IsUI);
            }

            return instance;
        }
    }
}
