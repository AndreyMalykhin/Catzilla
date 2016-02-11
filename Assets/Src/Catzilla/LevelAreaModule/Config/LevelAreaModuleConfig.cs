using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Config;
using Catzilla.CommonModule.Util;
using Catzilla.LevelModule.View;
using Catzilla.LevelAreaModule.Model;
using Catzilla.LevelAreaModule.View;
using Catzilla.LevelAreaModule.Controller;

namespace Catzilla.LevelAreaModule.Config {
    [CreateAssetMenuAttribute]
    public class LevelAreaModuleConfig: ModuleConfig {
        [SerializeField]
        private EnvTypeInfoStorage envTypeInfoStorage;

        public override void InitBindings(DiContainer container) {
            container.Bind<LevelAreaGenerator>().ToSingle();
            container.Bind<LevelAreaController>().ToSingle();
            container.Bind<EnvTypeInfoStorage>().ToInstance(envTypeInfoStorage);
            container.Bind<int>("EnvLayer").ToInstance(1 << 10);
        }

        public override void PostBindings(DiContainer container) {
            container.Inject(container.Resolve<EnvTypeInfoStorage>());
            var eventBus = container.Resolve<EventBus>();
            var levelAreaController =
                container.Resolve<LevelAreaController>();
            eventBus.On(
                LevelView.Event.Construct,
                levelAreaController.OnLevelConstruct);
            eventBus.On(LevelAreaView.Event.TriggerEnter,
                levelAreaController.OnTriggerEnter);
        }
    }
}
