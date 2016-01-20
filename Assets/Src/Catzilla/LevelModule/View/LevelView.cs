using UnityEngine;
using System.Collections.Generic;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.LevelModule.Model;
using Catzilla.LevelObjectModule.Model;
using Catzilla.LevelObjectModule.View;
using Catzilla.LevelAreaModule.Model;
using Catzilla.LevelAreaModule.View;

namespace Catzilla.LevelModule.View {
    public class LevelView: strange.extensions.mediation.impl.View {
        public enum Event {Construct}

        [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
        public IEventDispatcher EventBus {get; set;}

        [Inject("LevelAreaWidth")]
        public float AreaWidth {get; set;}

        [Inject("LevelAreaDepth")]
        public float AreaDepth {get; set;}

        public int Index {get; private set;}

        [SerializeField]
        private LevelAreaView areaProto;

        private float areaHalfWidth;
        private float areaHalfDepth;

        public void Init(int index) {
            Index = index;
        }

        [PostConstruct]
        public void OnConstruct() {
            Debug.Log("LevelView.OnConstruct()");
            areaHalfWidth = AreaWidth / 2f;
            areaHalfDepth = AreaDepth / 2f;
            EventBus.Dispatch(Event.Construct, this);
        }

        public LevelAreaView NewArea(int index) {
            var position = new Vector3(0f, 0f, index * AreaDepth);
            var areaView = (LevelAreaView) Instantiate(
                areaProto, position, Quaternion.identity);
            areaView.transform.parent = transform;
            return areaView;
        }

        public LevelObjectView NewObject(
            ObjectTypeInfo typeInfo, LevelAreaPoint spawnPoint, int areaIndex) {
            // Debug.LogFormat("LevelView.NewObject(); type={0}", typeInfo.Type);
            var position = new Vector3(
                spawnPoint.X + typeInfo.Width / 2f - areaHalfWidth,
                0f,
                spawnPoint.Z + typeInfo.Depth / 2f + areaIndex * AreaDepth - areaHalfDepth);
            var obj = (LevelObjectView) Instantiate(
                typeInfo.ViewProto, position, Quaternion.identity);
            obj.transform.parent = transform;
            return obj;
        }
    }
}
