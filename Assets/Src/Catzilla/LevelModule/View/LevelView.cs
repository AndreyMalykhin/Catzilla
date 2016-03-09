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
        public enum Event {Construct}

        [Inject]
        public EventBus EventBus {get; set;}

        [Inject("LevelAreaDepth")]
        public float AreaDepth {get; set;}

        [Inject]
        public PoolStorageView PoolStorage {get; set;}

        [Inject]
        public IInstantiator Instantiator {get; set;}

        public int Index {get; private set;}

        private int nextAreaIndex = 0;

        [PostInject]
        public void OnConstruct() {
            // DebugUtils.Log("LevelView.OnConstruct()");
            EventBus.Fire(Event.Construct, new Evt(this));
        }

        public void Init(int index) {
            Index = index;
        }

        public LevelAreaView NewArea(EnvTypeInfo envTypeInfo) {
            var poolId =
                envTypeInfo.ViewProto.GetComponent<PoolableView>().PoolId;
            var area = PoolStorage.Take(poolId).GetComponent<LevelAreaView>();
            area.Init(nextAreaIndex);
            area.transform.position =
                new Vector3(0f, 0f, nextAreaIndex * AreaDepth);
            area.transform.rotation = Quaternion.identity;
            area.transform.parent = transform;
            ++nextAreaIndex;
            return area;
        }

        public LevelObjectView NewObject(
            LevelObjectView objectProto, Vector3 spawnPoint, int areaIndex) {
            // DebugUtils.Log("LevelView.NewObject()");
            var poolable = objectProto.GetComponent<PoolableView>();
            LevelObjectView obj = null;

            if (poolable == null) {
                obj = Instantiator.InstantiatePrefab(objectProto.gameObject)
                    .GetComponent<LevelObjectView>();
            } else {
                obj = PoolStorage.Take(poolable.PoolId)
                    .GetComponent<LevelObjectView>();
            }

            var position = new Vector3(
                spawnPoint.x,
                spawnPoint.y,
                spawnPoint.z + areaIndex * AreaDepth);
            obj.transform.position = position;
            obj.transform.parent = transform;
            return obj;
        }
    }
}
