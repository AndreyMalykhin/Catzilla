using UnityEngine;
using System.Collections.Generic;
using Zenject;
using Catzilla.CommonModule.Util;

namespace Catzilla.CommonModule.View {
    public class PoolStorageView: MonoBehaviour {
        [System.Serializable]
        private struct PoolParams {
            public PoolableView ViewProto;
            public int InitialSize;
        }

        [Inject]
        private IInstantiator instantiator;

        [SerializeField]
        private Transform instanceContainer;

        [SerializeField]
        private PoolParams[] poolsParams;

        private readonly IDictionary<int, Pool<PoolableView>> poolsMap =
            new Dictionary<int, Pool<PoolableView>>(64);
        private readonly List<PoolableView> returnList =
            new List<PoolableView>(32);

        public PoolableView Take(int poolId) {
            PoolableView instance = poolsMap[poolId].Take();

            if (instance.transform.parent == instanceContainer) {
                instance.transform.SetParent(null, !instance.IsUI);
            }

            return instance;
        }

        public void Return(PoolableView instance) {
            DebugUtils.Assert(!IsInPool(instance));
            poolsMap[instance.PoolId].Return(instance);
            instance.transform.SetParent(instanceContainer, !instance.IsUI);
            // DebugUtils.Log("PoolStorageView.Return(); left={0}; instance={1}",
            //     poolsMap[instance.PoolId].available, instance);
        }

        public void ReturnLater(PoolableView instance) {
            returnList.Add(instance);
        }

        public void Add(int poolId, int count) {
            // DebugUtils.Log("PoolStorageView.Add()");
            poolsMap[poolId].Add(count);
        }

        public void Refill() {
            // DebugUtils.Log("PoolStorageView.Refill()");
            foreach (KeyValuePair<int, Pool<PoolableView>> item in poolsMap) {
                var pool = item.Value;
                pool.Add(pool.Capacity - pool.Size);
            }
        }

        [PostInject]
        public void OnConstruct() {
            // DebugUtils.Log("PoolStorageView.OnConstruct()");
            DebugUtils.Assert(instanceContainer.gameObject != gameObject);
            instanceContainer.gameObject.SetActive(false);

            for (int i = 0; i < poolsParams.Length; ++i) {
                PoolParams poolParams = poolsParams[i];
                var instanceProvider = new ViewInstanceProvider(
                    poolParams.ViewProto, instanceContainer, instantiator);
                var pool = new Pool<PoolableView>(
                    instanceProvider, poolParams.InitialSize);
                poolsMap.Add(poolParams.ViewProto.PoolId, pool);
            }
        }

        private void LateUpdate() {
            for (int i = returnList.Count - 1; i >= 0; --i) {
                PoolableView instance = returnList[i];
                returnList.RemoveAt(i);

                if (IsInPool(instance)) {
                    continue;
                }

                Return(instance);
            }
        }

        private bool IsInPool(PoolableView instance) {
            return instance.transform.parent == instanceContainer;
        }
    }
}
