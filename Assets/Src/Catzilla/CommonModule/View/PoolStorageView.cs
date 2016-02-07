using UnityEngine;
using System.Collections.Generic;
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

        private readonly IDictionary<int, Pool<PoolableView>> poolsMap =
            new Dictionary<int, Pool<PoolableView>>();

        public PoolableView Take(int poolId) {
            PoolableView instance = poolsMap[poolId].Take();
            instance.transform.SetParent(null, !instance.IsUI);
            instance.gameObject.SetActive(true);
            return instance;
        }

        public void Return(PoolableView instance) {
            instance.transform.SetParent(transform, !instance.IsUI);
            instance.gameObject.SetActive(false);
            poolsMap[instance.PoolId].Return(instance);
            // DebugUtils.Log("PoolStorageView.Return(); left={0}; instance={1}",
            //     poolsMap[instance.PoolId].available, instance);
        }

        public void Reset() {
            // DebugUtils.Log("PoolStorageView.Reset()");

            foreach (Transform child in transform) {
                GameObject.Destroy(child.gameObject);
            }

            foreach (KeyValuePair<int, Pool<PoolableView>> item in poolsMap) {
                var pool = item.Value;
                int poolSize = pool.Size;
                pool.Clear();
                pool.Add(poolSize);
            }
        }

        private void Awake() {
            for (int i = 0; i < poolsParams.Length; ++i) {
                PoolParams poolParams = poolsParams[i];
                var instanceProvider =
                    new ViewInstanceProvider(poolParams.ViewProto, transform);
                var pool = new Pool<PoolableView>(
                    instanceProvider, poolParams.InitialSize);
                pool.Add(poolParams.InitialSize);
                poolsMap.Add(poolParams.ViewProto.PoolId, pool);
            }
        }
    }
}
