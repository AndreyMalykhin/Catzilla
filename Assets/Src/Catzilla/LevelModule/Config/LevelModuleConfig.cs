using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Config;
using Catzilla.CommonModule.Util;
using Catzilla.LevelAreaModule.View;
using Catzilla.LevelModule.Controller;
using Catzilla.LevelModule.Model;
using Catzilla.LevelModule.View;

namespace Catzilla.LevelModule.Config {
    [CreateAssetMenuAttribute]
    public class LevelModuleConfig: ModuleConfig {
        [SerializeField]
        private LevelSettingsStorage levelSettingsStorage;

        public override void InitBindings(DiContainer container) {
            var levelStartScreen = GameObject.FindWithTag("LevelStartScreen")
                .GetComponent<LevelStartScreenView>();
            container.Bind<LevelStartScreenView>().ToInstance(levelStartScreen);
            var levelCompleteScreen =
                GameObject.FindWithTag("LevelCompleteScreen")
                .GetComponent<LevelCompleteScreenView>();
            container.Bind<LevelCompleteScreenView>()
                .ToInstance(levelCompleteScreen);
            container.Bind<LevelSettingsStorage>()
                .ToInstance(levelSettingsStorage);
            container.Bind<LevelController>().ToSingle();
            container.Bind<LevelCompleteScreenController>().ToSingle();
            container.Bind<LevelGenerator>().ToSingle();
            float areaWidth = 16f;
            float areaDepth = 24f;
            container.Bind<float>("LevelAreaWidth").ToInstance(areaWidth);
            container.Bind<float>("LevelAreaDepth").ToInstance(areaDepth);
            container.Bind<float>("LevelMinX").ToInstance(-areaWidth / 2f);
            container.Bind<float>("LevelMaxX").ToInstance(areaWidth / 2f);
            container.Bind<string>("LevelScene").ToInstance("Level");
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
