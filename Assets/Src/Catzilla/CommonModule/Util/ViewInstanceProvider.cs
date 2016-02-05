using UnityEngine;
using System;
using System.Collections;
using strange.framework.api;
using Catzilla.CommonModule.View;

namespace Catzilla.CommonModule.Util {
    public class ViewInstanceProvider: IInstanceProvider {
        private PoolableView proto;
        private Transform parent;

        public ViewInstanceProvider(PoolableView proto, Transform parent) {
            this.proto = proto;
            this.parent = parent;
        }

        public T GetInstance<T>() {
            object instance = GetInstance(typeof(T));
            return (T) instance;
        }

        public object GetInstance(Type key) {
            var instance = (PoolableView) GameObject.Instantiate(proto);
            instance.transform.SetParent(parent, !instance.IsUI);
            instance.gameObject.SetActive(false);
            return instance;
        }
    }
}
