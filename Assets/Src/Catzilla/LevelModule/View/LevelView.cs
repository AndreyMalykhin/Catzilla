using UnityEngine;
using System.Collections.Generic;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.View;
using Catzilla.CommonModule.Util;
using Catzilla.LevelObjectModule.Model;
using Catzilla.LevelObjectModule.View;
using Catzilla.LevelAreaModule.Model;
using Catzilla.LevelAreaModule.View;
using Catzilla.LevelModule.Model;

namespace Catzilla.LevelModule.View {
    public class LevelView: strange.extensions.mediation.impl.View {
        public enum Event {Construct}

        [Inject]
        public EventBus EventBus {get; set;}

        [Inject("LevelAreaDepth")]
        public float AreaDepth {get; set;}

        [Inject]
        public PoolStorageView PoolStorage {get; set;}

        public int Index {get; private set;}

        public void Init(int index) {
            Index = index;
        }

        [PostConstruct]
        public void OnConstruct() {
            // DebugUtils.Log("LevelView.OnConstruct()");
            EventBus.Fire(Event.Construct, new Evt(this));
        }

        public void NewArea(int index, EnvTypeInfo envTypeInfo) {
            var poolId =
                envTypeInfo.ViewProto.GetComponent<PoolableView>().PoolId;
            var area = PoolStorage.Get(poolId);
            area.transform.position = new Vector3(0f, 0f, index * AreaDepth);
            area.transform.rotation = Quaternion.identity;
            area.transform.parent = transform;
        }

        public LevelObjectView NewObject(
            ObjectTypeInfo typeInfo, Vector3 spawnPoint, int areaIndex) {
            // DebugUtils.Log("LevelView.NewObject(); type={0}", typeInfo.Type);
            var position = new Vector3(
                spawnPoint.x,
                spawnPoint.y,
                spawnPoint.z + areaIndex * AreaDepth);
            var poolable = typeInfo.ViewProto.GetComponent<PoolableView>();
            LevelObjectView obj;

            if (poolable == null) {
                obj = (LevelObjectView) Instantiate(
                    typeInfo.ViewProto, position, Quaternion.identity);
            } else {
                obj = PoolStorage.Get(poolable.PoolId)
                    .GetComponent<LevelObjectView>();
            }

            obj.transform.position = position;
            obj.transform.rotation = Quaternion.identity;
            obj.transform.parent = transform;
            return obj;
        }
    }
}
