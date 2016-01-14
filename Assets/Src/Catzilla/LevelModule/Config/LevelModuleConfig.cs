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
            injectionBinder.Bind<LevelController>()
                .To<LevelController>()
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
                LevelView.Event.Ready, levelController.OnViewReady);
            eventBus.AddListener(LevelAreaView.Event.TriggerEnter,
                levelController.OnAreaTriggerEnter);
            // eventBus.AddListener(LevelAreaView.Event.TriggerExit,
            //     levelController.OnAreaTriggerExit);
        }
    }
}
