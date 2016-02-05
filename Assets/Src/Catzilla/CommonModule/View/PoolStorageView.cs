using UnityEngine;
using System.Collections.Generic;
using strange.extensions.pool.api;
using strange.extensions.pool.impl;
using Catzilla.CommonModule.Util;

namespace Catzilla.CommonModule.View {
    public class PoolStorageView: MonoBehaviour {
        [System.Serializable]
        private struct PoolParams {
            public PoolableView ViewProto;
            public int InitialSize;
        }

        [SerializeField]
        private PoolParams[] poolsParams;

        private readonly IDictionary<int, IPool<PoolableView>> poolsMap =
            new Dictionary<int, IPool<PoolableView>>();

        public PoolableView Get(int poolId) {
            PoolableView instance = poolsMap[poolId].GetInstance();
            instance.transform.SetParent(null, !instance.IsUI);
            instance.gameObject.SetActive(true);
            return instance;
        }

        public void Return(PoolableView instance) {
            instance.transform.SetParent(transform, !instance.IsUI);
            instance.gameObject.SetActive(false);
            poolsMap[instance.PoolId].ReturnInstance(instance);
            // DebugUtils.Log("PoolStorageView.Return(); left={0}; instance={1}",
            //     poolsMap[instance.PoolId].available, instance);
        }

        public void Delete(PoolableView instance) {
            // DebugUtils.Log("PoolStorageView.Delete(); left={0}; instance={1}",
            //     poolsMap[instance.PoolId].instanceCount, instance);
            poolsMap[instance.PoolId].Remove(instance);
        }

        public void Reset() {
            // DebugUtils.Log("PoolStorageView.Reset()");

            foreach (Transform child in transform) {
                GameObject.Destroy(child.gameObject);
            }

            foreach (KeyValuePair<int, IPool<PoolableView>> item in poolsMap) {
                var pool = item.Value;
                int instancesCount = pool.instanceCount;
                pool.Clean();
                Fill(pool, instancesCount);
            }
        }

        private void Awake() {
            for (int i = 0; i < poolsParams.Length; ++i) {
                PoolParams poolParams = poolsParams[i];
                var instanceProvider =
                    new ViewInstanceProvider(poolParams.ViewProto, transform);
                var pool = new Pool<PoolableView>{
                    instanceProvider = instanceProvider
                };
                Fill(pool, poolParams.InitialSize);
                poolsMap.Add(poolParams.ViewProto.PoolId, pool);
            }
        }

        private void Fill(IPool<PoolableView> pool, int instancesCount) {
            for (int i = 0; i < instancesCount; ++i) {
                var instance =
                    (PoolableView) pool.instanceProvider.GetInstance(null);
                pool.Add(instance);
            }
        }
    }
}
