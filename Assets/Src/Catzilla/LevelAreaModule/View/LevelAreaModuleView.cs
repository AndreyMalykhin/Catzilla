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

        public override void InitBindings(DiContainer container) {
            container.Bind<LevelAreaGenerator>().ToSingle();
            container.Bind<LevelAreaController>().ToSingle();
            container.Bind<EnvTypeInfoStorage>().ToInstance(envTypeInfoStorage);
        }

        public override void PostBindings(DiContainer container) {
            container.Inject(container.Resolve<EnvTypeInfoStorage>());
            var eventBus = container.Resolve<EventBus>();
            var levelAreaController =
                container.Resolve<LevelAreaController>();
            eventBus.On((int) Events.LevelConstruct,
                levelAreaController.OnLevelConstruct);
            eventBus.On((int) Events.PlayerConstruct,
                levelAreaController.OnPlayerConstruct);
            eventBus.On((int) Events.LevelAreaTriggerEnter,
                levelAreaController.OnTriggerEnter);
        }
    }
}
