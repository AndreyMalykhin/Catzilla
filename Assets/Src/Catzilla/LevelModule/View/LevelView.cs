using UnityEngine;
using System.Collections.Generic;
using Zenject;
using Catzilla.CommonModule.View;
using Catzilla.CommonModule.Util;
using Catzilla.LevelObjectModule.Model;
using Catzilla.LevelObjectModule.View;
using Catzilla.LevelAreaModule.Model;
using Catzilla.LevelAreaModule.View;
using Catzilla.LevelModule.Model;

namespace Catzilla.LevelModule.View {
    public class LevelView: MonoBehaviour {
        [Inject]
        private EventBus eventBus;

        [Inject("LevelAreaDepth")]
        private float areaDepth;

        [Inject]
        private PoolStorageView poolStorage;

        [Inject]
        private IInstantiator instantiator;

        public int Index {get; private set;}
        public int NextAreaIndex {get {return nextAreaIndex;}}

        private int nextAreaIndex = 0;

        [PostInject]
        public void OnConstruct() {
            // DebugUtils.Log("LevelView.OnConstruct()");
            eventBus.Fire((int) Events.LevelConstruct, new Evt(this));
        }

        public void Init(int index) {
            Index = index;
        }

        public LevelAreaView NewArea(EnvTypeInfo envTypeInfo) {
            Profiler.BeginSample("LevelView.NewArea()");
            var poolId =
                envTypeInfo.ViewProto.GetComponent<PoolableView>().PoolId;
            var area = poolStorage.Take(poolId).GetComponent<LevelAreaView>();
            area.Init(nextAreaIndex);
            area.transform.position =
                new Vector3(0f, 0f, nextAreaIndex * areaDepth);
            area.transform.rotation = Quaternion.identity;
            area.transform.parent = transform;
            ++nextAreaIndex;
            Profiler.EndSample();
            return area;
        }

        public LevelObjectView NewObject(
            LevelObjectView objectProto, Vector3 spawnPoint, int areaIndex) {
            // DebugUtils.Log("LevelView.NewObject()");
            var poolable = objectProto.GetComponent<PoolableView>();
            LevelObjectView obj = null;

            if (poolable == null) {
                obj = instantiator.InstantiatePrefab(objectProto.gameObject)
                    .GetComponent<LevelObjectView>();
            } else {
                obj = poolStorage.Take(poolable.PoolId)
                    .GetComponent<LevelObjectView>();
            }

            var position = new Vector3(
                spawnPoint.x,
                spawnPoint.y,
                spawnPoint.z + areaIndex * areaDepth);
            obj.transform.position = position;
            obj.transform.parent = transform;
            return obj;
        }
    }
}
