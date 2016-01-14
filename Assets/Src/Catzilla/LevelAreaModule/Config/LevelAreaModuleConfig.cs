using UnityEngine;
using System.Collections;
using strange.extensions.injector.api;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.Config;
using Catzilla.LevelAreaModule.Model;

namespace Catzilla.LevelAreaModule.Config {
    public class LevelAreaModuleConfig: IModuleConfig {
        void IModuleConfig.InitBindings(IInjectionBinder injectionBinder) {
            injectionBinder.Bind<LevelAreaGenerator>()
                .To<LevelAreaGenerator>()
                .ToSingleton()
                .CrossContext();
            injectionBinder.Bind<EnvFactory>()
                .To<EnvFactory>()
                .ToSingleton()
                .CrossContext();
            injectionBinder.Bind<EnvTypeInfoStorage>()
                .To<EnvTypeInfoStorage>()
                .ToSingleton()
                .CrossContext();
            injectionBinder.Bind<int>()
                .ToValue(1 << 10)
                .ToName("EnvLayer")
                .ToInject(false)
                .CrossContext();
        }

        void IModuleConfig.PostBindings(IInjectionBinder injectionBinder) {
            // var eventBus = injectionBinder.GetInstance<IEventDispatcher>(
            //     ContextKeys.CROSS_CONTEXT_DISPATCHER);
        }
    }
}
