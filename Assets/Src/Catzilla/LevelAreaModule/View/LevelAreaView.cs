using UnityEngine;
using System.Collections.Generic;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.View;
using Catzilla.CommonModule.Util;
using Catzilla.LevelAreaModule.Model;
using Catzilla.LevelObjectModule.Model;
using Catzilla.LevelObjectModule.View;

namespace Catzilla.LevelAreaModule.View {
    public class LevelAreaView: strange.extensions.mediation.impl.View {
        public enum Event {TriggerEnter, TriggerExit}

        [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
        public IEventDispatcher EventBus {get; set;}

        [Inject]
        public EnvTypeInfoStorage EnvTypeInfoStorage {get; set;}

        [Inject]
        public ObjectTypeInfoStorage ObjectTypeInfoStorage {get; set;}

        [Inject("LevelAreaWidth")]
        public float Width {get; set;}

        [Inject("LevelAreaDepth")]
        public float Depth {get; set;}

        [Inject("PlayerObjectType")]
        public LevelObjectType PlayerObjectType {get; set;}

        public int Index {get {return area.Index;}}

        private const float CellSize = 1f;
        private float halfWidth;
        private float halfDepth;
        private LevelArea area;

        [PostConstruct]
        public void OnReady() {
            halfDepth = Depth / 2f;
            halfWidth = Width / 2f;
        }

        public void Init(LevelArea area) {
            this.area = area;
        }

        protected override void Start() {
            base.Start();
            Render();
        }

        private void OnTriggerEnter(Collider collider) {
            EventBus.Dispatch(Event.TriggerEnter, collider);
        }

        private void OnTriggerExit(Collider collider) {
            EventBus.Dispatch(Event.TriggerExit, new EventData(this, collider));
        }

        private void Render() {
            Debug.Log("LevelAreaView.Render()");
            RenderEnv(area.Env);
            List<LevelObject> objects = area.Objects;
            int objectsCount = objects.Count;

            for (int i = 0; i < objectsCount; ++i) {
                RenderObject(objects[i]);
            }
        }

        private void RenderEnv(Env env) {
            var envView = (EnvView) Instantiate(
                EnvTypeInfoStorage.Get(env.Type).ViewProto,
                transform.position,
                Quaternion.identity);
            envView.transform.parent = transform;
            envView.Init(env);
            envView.GetComponent<DisposableView>().Init(gameObject);
        }

        private void RenderObject(LevelObject obj) {
            ObjectTypeInfo objTypeInfo = ObjectTypeInfoStorage.Get(obj.Type);
            var position = new Vector3(
                obj.SpawnPoint.X - halfWidth + objTypeInfo.Width / 2f,
                0f,
                obj.SpawnPoint.Z - halfDepth + objTypeInfo.Depth / 2f + area.Index * Depth);
            var objView = (LevelObjectView) Instantiate(objTypeInfo.ViewProto,
                position, Quaternion.identity);
            objView.transform.parent = transform.parent;
            objView.Init(obj);
        }
    }
}
