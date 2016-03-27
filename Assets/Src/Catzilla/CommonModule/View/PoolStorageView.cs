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

        [SerializeField]
        private PoolParams[] poolsParams;

        [Inject]
        private IInstantiator instantiator;

        private readonly IDictionary<int, Pool<PoolableView>> poolsMap =
            new Dictionary<int, Pool<PoolableView>>();
        private readonly List<PoolableView> returnList =
            new List<PoolableView>(32);

        public PoolableView Take(int poolId) {
            PoolableView instance = poolsMap[poolId].Take();
            instance.transform.SetParent(null, !instance.IsUI);
            instance.gameObject.SetActive(instance.ActivateOnTake);
            return instance;
        }

        public void Return(PoolableView instance) {
            DebugUtils.Assert(!IsInPool(instance));
            poolsMap[instance.PoolId].Return(instance);
            instance.ActivateOnTake = instance.gameObject.activeSelf;
            instance.transform.SetParent(transform, !instance.IsUI);
            instance.gameObject.SetActive(false);
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

            for (int i = 0; i < poolsParams.Length; ++i) {
                PoolParams poolParams = poolsParams[i];
                var instanceProvider = new ViewInstanceProvider(
                    poolParams.ViewProto, transform, instantiator);
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
            return instance.transform.parent == transform;
        }
    }
}
