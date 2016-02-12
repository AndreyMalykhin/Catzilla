using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.View;
using Catzilla.CommonModule.Util;
using Catzilla.LevelAreaModule.View;
using Catzilla.LevelModule.Controller;
using Catzilla.LevelModule.Model;
using Catzilla.LevelModule.View;

namespace Catzilla.LevelModule.View {
    public class LevelModuleView: ModuleView {
        [SerializeField]
        private LevelSettingsStorage levelSettingsStorage;

        [SerializeField]
        private LevelStartScreenView startScreen;

        [SerializeField]
        private LevelCompleteScreenView completeScreen;

        [SerializeField]
        private float areaWidth;

        [SerializeField]
        private float areaDepth;

        [SerializeField]
        private string levelScene;

        [SerializeField]
        private string levelTag;

        public override void InitBindings(DiContainer container) {
            container.Bind<LevelStartScreenView>().ToInstance(startScreen);
            container.Bind<LevelCompleteScreenView>()
                .ToInstance(completeScreen);
            container.Bind<LevelSettingsStorage>()
                .ToInstance(levelSettingsStorage);
            container.Bind<LevelController>().ToSingle();
            container.Bind<LevelCompleteScreenController>().ToSingle();
            container.Bind<LevelGenerator>().ToSingle();
            container.Bind<float>("LevelAreaWidth").ToInstance(areaWidth);
            container.Bind<float>("LevelAreaDepth").ToInstance(areaDepth);
            container.Bind<float>("LevelMinX").ToInstance(-areaWidth / 2f);
            container.Bind<float>("LevelMaxX").ToInstance(areaWidth / 2f);
            container.Bind<string>("LevelScene").ToInstance(levelScene);
            container.Bind<string>("LevelTag").ToInstance(levelTag);
        }

        public override void PostBindings(DiContainer container) {
            container.Inject(container.Resolve<LevelSettingsStorage>());
            var eventBus = container.Resolve<EventBus>();

            var levelController =
                container.Resolve<LevelController>();
            eventBus.On(
                LevelView.Event.Construct, levelController.OnViewConstruct);

            var levelCompleteScreenController =
                container.Resolve<LevelCompleteScreenController>();
            eventBus.On(LevelCompleteScreenView.Event.Show,
                levelCompleteScreenController.OnShow);
        }
    }
}
