using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;
using Catzilla.LevelModule.View;
using Catzilla.LevelObjectModule.View;
using Catzilla.LevelAreaModule.Model;
using Catzilla.LevelAreaModule.View;
using Catzilla.LevelAreaModule.Controller;

namespace Catzilla.LevelAreaModule.View {
    public class LevelAreaModuleView: ModuleView {
        [SerializeField]
        private EnvTypeInfoStorage envTypeInfoStorage;

        [SerializeField]
        private int envLayer;

        public override void InitBindings(DiContainer container) {
            container.Bind<LevelAreaGenerator>().ToSingle();
            container.Bind<LevelAreaController>().ToSingle();
            container.Bind<EnvTypeInfoStorage>().ToInstance(envTypeInfoStorage);
            container.Bind<int>("EnvLayer").ToInstance(1 << envLayer);
        }

        public override void PostBindings(DiContainer container) {
            container.Inject(container.Resolve<EnvTypeInfoStorage>());
            var eventBus = container.Resolve<EventBus>();
            var levelAreaController =
                container.Resolve<LevelAreaController>();
            eventBus.On(LevelView.Event.Construct,
                levelAreaController.OnLevelConstruct);
            eventBus.On(PlayerView.Event.Construct,
                levelAreaController.OnPlayerConstruct);
            eventBus.On(LevelAreaView.Event.TriggerEnter,
                levelAreaController.OnTriggerEnter);
        }
    }
}
