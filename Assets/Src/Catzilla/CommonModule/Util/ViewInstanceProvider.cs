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

        public ViewInstanceProvider(
            PoolableView proto, Transform parent, IInstantiator instantiator) {
            this.proto = proto;
            this.parent = parent;
            this.instantiator = instantiator;
        }

        public PoolableView Get() {
            var instance = instantiator.InstantiatePrefab(proto.gameObject)
                .GetComponent<PoolableView>();
            instance.transform.SetParent(parent, !instance.IsUI);
            instance.ActivateOnTake = instance.gameObject.activeSelf;
            instance.gameObject.SetActive(false);
            return instance;
        }
    }
}
