using UnityEngine;
using System;
using System.Collections;
using Catzilla.CommonModule.View;

namespace Catzilla.CommonModule.Util {
    public class ViewInstanceProvider: Pool<PoolableView>.IInstanceProvider {
        private readonly PoolableView proto;
        private readonly Transform parent;

        public ViewInstanceProvider(PoolableView proto, Transform parent) {
            this.proto = proto;
            this.parent = parent;
        }

        public PoolableView Get() {
            var instance = (PoolableView) GameObject.Instantiate(proto);
            instance.transform.SetParent(parent, !instance.IsUI);
            instance.gameObject.SetActive(false);
            return instance;
        }
    }
}
