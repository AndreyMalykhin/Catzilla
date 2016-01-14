using UnityEngine;
using System.Collections;
using strange.extensions.injector.api;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.Config;
using Catzilla.PlayerModule.Model;

namespace Catzilla.PlayerModule.Config {
    public class PlayerModuleConfig: IModuleConfig {
        void IModuleConfig.InitBindings(IInjectionBinder injectionBinder) {
            injectionBinder.Bind<PlayerSettingsStorage>()
                .To<PlayerSettingsStorage>()
                .ToSingleton()
                .CrossContext();
        }

        void IModuleConfig.PostBindings(IInjectionBinder injectionBinder) {}
    }
}
