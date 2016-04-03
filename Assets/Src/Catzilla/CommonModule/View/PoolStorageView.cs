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
            Pool<PoolableView> pool = poolsMap[poolId];

            if (pool.Size <= 0) {
                DebugUtils.Log("PoolStorageView.Take(); poolId={0}", poolId);
                DebugUtils.Assert(false);
            }

            PoolableView instance = pool.Take();

            if (instance.DeactivateOnReturn) {
                instance.gameObject.SetActive(instance.ActivateOnTake);
            }

            if (instanceContainer != null
                && instance.transform.parent == instanceContainer) {
                instance.transform.SetParent(null, !instance.IsUI);
            }

            instance.IsInPool = false;
            return instance;
        }

        public void Return(PoolableView instance) {
            DebugUtils.Assert(!IsInPool(instance));
            poolsMap[instance.PoolId].Return(instance);
            GameObject instanceObject = instance.gameObject;

            if (instance.DeactivateOnReturn) {
                instance.ActivateOnTake = instanceObject.activeSelf;
                instanceObject.SetActive(false);
            }

            if (instanceContainer != null) {
                instance.transform.SetParent(instanceContainer, !instance.IsUI);
            }

            instance.IsInPool = true;
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
            foreach (Pool<PoolableView> pool in poolsMap.Values) {
                pool.Add(pool.Capacity - pool.Size);
            }
        }

        public void Cleanup() {
            // DebugUtils.Log("PoolStorageView.Cleanup()");
            var validInstances = new List<PoolableView>(64);

            foreach (Pool<PoolableView> pool in poolsMap.Values) {
                List<PoolableView> instances = pool.Instances;

                for (int j = 0; j < instances.Count; ++j) {
                    var instance = instances[j];

                    if (instance == null) {
                        continue;
                    }

                    validInstances.Add(instance);
                }

                pool.Clear();

                for (int i = 0; i < validInstances.Count; ++i) {
                    pool.Add(validInstances[i]);
                }

                validInstances.Clear();
            }
        }

        [PostInject]
        public void OnConstruct() {
            // DebugUtils.Log("PoolStorageView.OnConstruct()");
            for (int i = 0; i < poolsParams.Length; ++i) {
                PoolParams poolParams = poolsParams[i];
                var instanceProvider = new ViewInstanceProvider(
                    poolParams.ViewProto, instantiator, instanceContainer);
                var pool = new Pool<PoolableView>(instanceProvider,
                    poolParams.InitialSize);
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
            return instance.IsInPool;
        }
    }
}
