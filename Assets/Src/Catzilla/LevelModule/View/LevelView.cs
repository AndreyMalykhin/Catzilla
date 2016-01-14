using UnityEngine;
using System.Collections.Generic;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.LevelModule.Model;
using Catzilla.LevelAreaModule.Model;
using Catzilla.LevelAreaModule.View;

namespace Catzilla.LevelModule.View {
    public class LevelView: strange.extensions.mediation.impl.View {
        public enum Event {Ready}

        [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
        public IEventDispatcher EventBus {get; set;}

        [Inject("LevelAreaDepth")]
        public float AreaDepth {get; set;}

        public LevelAreaView AreaProto;

        // private List<LevelAreaView> areas = new List<LevelAreaView>(4);
        private Level level;

        [PostConstruct]
        public void OnReady() {
            Debug.Log("LevelView.OnReady()");
            EventBus.Dispatch(Event.Ready, this);
        }

        public void Init(Level level) {
            this.level = level;
        }

        public void AddArea(LevelArea area) {
            Debug.Log("LevelView.AddArea()");
            RenderArea(area);
        }

        // public void DeleteArea(int index) {
        //     Debug.Log("LevelView.DeleteArea()");

        //     for (int i = 0; i < areas.Count; ++i) {
        //         LevelAreaView area = areas[i];

        //         if (area.Index == index) {
        //             areas.Remove(area);
        //             Destroy(area.gameObject);
        //             break;
        //         }
        //     }
        // }

        protected override void Start() {
            base.Start();
            Render();
        }

        private void Render() {
            Debug.Log("LevelView.Render()");
            List<LevelArea> areas = level.Areas;

            for (int i = 0; i < areas.Count; ++i) {
                RenderArea(areas[i]);
            }
        }

        private void RenderArea(LevelArea area) {
            var position =
                new Vector3(0f, 0f, area.Index * AreaDepth);
            var areaView = (LevelAreaView) Instantiate(
                AreaProto, position, Quaternion.identity);
            areaView.transform.parent = transform;
            areaView.Init(area);
            // areas.Add(areaView);
        }
    }
}
