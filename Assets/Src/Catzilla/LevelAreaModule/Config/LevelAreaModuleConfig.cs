using UnityEngine;
using System.Collections;
using strange.extensions.injector.api;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.Config;
using Catzilla.LevelModule.View;
using Catzilla.LevelAreaModule.Model;
using Catzilla.LevelAreaModule.View;
using Catzilla.LevelAreaModule.Controller;

namespace Catzilla.LevelAreaModule.Config {
    public class LevelAreaModuleConfig: IModuleConfig {
        void IModuleConfig.InitBindings(IInjectionBinder injectionBinder) {
            injectionBinder.Bind<LevelAreaGenerator>()
                .To<LevelAreaGenerator>()
                .ToSingleton()
                .CrossContext();
            injectionBinder.Bind<LevelAreaController>()
                .To<LevelAreaController>()
                .ToSingleton()
                .CrossContext();
            injectionBinder.Bind<EnvTypeInfoStorage>()
                .ToValue(Resources.Load<EnvTypeInfoStorage>(
                    "EnvTypeInfoStorage"))
                .ToInject(false)
                .CrossContext();
            injectionBinder.Bind<int>()
                .ToValue(1 << 10)
                .ToName("EnvLayer")
                .ToInject(false)
                .CrossContext();
        }

        void IModuleConfig.PostBindings(IInjectionBinder injectionBinder) {
            var eventBus = injectionBinder.GetInstance<IEventDispatcher>(
                ContextKeys.CROSS_CONTEXT_DISPATCHER);
            var levelAreaController =
                injectionBinder.GetInstance<LevelAreaController>();
            eventBus.AddListener(
                LevelView.Event.Ready, levelAreaController.OnLevelReady);
            eventBus.AddListener(LevelAreaView.Event.TriggerEnter,
                levelAreaController.OnTriggerEnter);
        }
    }
}
