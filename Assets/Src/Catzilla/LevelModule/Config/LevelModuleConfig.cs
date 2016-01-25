using UnityEngine;
using System.Collections;
using strange.extensions.injector.api;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.Config;
using Catzilla.LevelModule.Controller;
using Catzilla.LevelModule.Model;
using Catzilla.LevelModule.View;
using Catzilla.LevelAreaModule.View;

namespace Catzilla.LevelModule.Config {
    public class LevelModuleConfig: IModuleConfig {
        void IModuleConfig.InitBindings(IInjectionBinder injectionBinder) {
            var levelStartScreen = GameObject.FindWithTag("LevelStartScreen")
                .GetComponent<LevelStartScreenView>();
            injectionBinder.Bind<LevelStartScreenView>()
                .ToValue(levelStartScreen)
                .ToInject(false)
                .CrossContext();
            var levelCompleteScreen =
                GameObject.FindWithTag("LevelCompleteScreen")
                .GetComponent<LevelCompleteScreenView>();
            injectionBinder.Bind<LevelCompleteScreenView>()
                .ToValue(levelCompleteScreen)
                .ToInject(false)
                .CrossContext();
            injectionBinder.Bind<LevelSettingsStorage>()
                .ToValue(Resources.Load<LevelSettingsStorage>(
                    "LevelSettingsStorage"))
                .ToInject(false)
                .CrossContext();
            injectionBinder.Bind<LevelController>()
                .To<LevelController>()
                .ToSingleton()
                .CrossContext();
            injectionBinder.Bind<LevelCompleteScreenController>()
                .To<LevelCompleteScreenController>()
                .ToSingleton()
                .CrossContext();
            injectionBinder.Bind<LevelStartScreenController>()
                .To<LevelStartScreenController>()
                .ToSingleton()
                .CrossContext();
            injectionBinder.Bind<LevelGenerator>()
                .To<LevelGenerator>()
                .ToSingleton()
                .CrossContext();
            float areaWidth = 16f;
            float areaDepth = 24f;
            injectionBinder.Bind<float>()
                .ToValue(areaWidth)
                .ToName("LevelAreaWidth")
                .ToInject(false)
                .CrossContext();
            injectionBinder.Bind<float>()
                .ToValue(areaDepth)
                .ToName("LevelAreaDepth")
                .ToInject(false)
                .CrossContext();
            injectionBinder.Bind<float>()
                .ToValue(-areaWidth / 2f)
                .ToName("LevelMinX")
                .ToInject(false)
                .CrossContext();
            injectionBinder.Bind<float>()
                .ToValue(areaWidth / 2f)
                .ToName("LevelMaxX")
                .ToInject(false)
                .CrossContext();
            injectionBinder.Bind<string>()
                .ToValue("Level")
                .ToName("LevelScene")
                .ToInject(false)
                .CrossContext();
        }

        void IModuleConfig.PostBindings(IInjectionBinder injectionBinder) {
            var eventBus = injectionBinder.GetInstance<IEventDispatcher>(
                ContextKeys.CROSS_CONTEXT_DISPATCHER);

            var levelController =
                injectionBinder.GetInstance<LevelController>();
            eventBus.AddListener(
                LevelView.Event.Construct, levelController.OnViewConstruct);

            var levelCompleteScreenController =
                injectionBinder.GetInstance<LevelCompleteScreenController>();
            eventBus.AddListener(LevelCompleteScreenView.Event.Hide,
                levelCompleteScreenController.OnHide);
            eventBus.AddListener(LevelCompleteScreenView.Event.Show,
                levelCompleteScreenController.OnShow);

            var levelStartScreenController =
                injectionBinder.GetInstance<LevelStartScreenController>();
            eventBus.AddListener(LevelStartScreenView.Event.Hide,
                levelStartScreenController.OnHide);
            eventBus.AddListener(LevelView.Event.Construct,
                levelStartScreenController.OnLevelConstruct);
        }
    }
}
