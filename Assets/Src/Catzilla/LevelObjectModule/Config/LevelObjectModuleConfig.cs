using UnityEngine;
using System.Collections;
using strange.extensions.injector.api;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.Config;
using Catzilla.LevelObjectModule.Model;
using Catzilla.LevelObjectModule.View;
using Catzilla.LevelObjectModule.Controller;

namespace Catzilla.LevelObjectModule.Config {
    public class LevelObjectModuleConfig: IModuleConfig {
        void IModuleConfig.InitBindings(IInjectionBinder injectionBinder) {
            injectionBinder.Bind<LevelObjectFactory>()
                .To<LevelObjectFactory>()
                .ToSingleton()
                .CrossContext();
            injectionBinder.Bind<ObjectTypeInfoStorage>()
                .To<ObjectTypeInfoStorage>()
                .ToSingleton()
                .CrossContext();
            injectionBinder.Bind<PlayerController>()
                .To<PlayerController>()
                .ToSingleton()
                .CrossContext();
            injectionBinder.Bind<SmashableController>()
                .To<SmashableController>()
                .ToSingleton()
                .CrossContext();
            injectionBinder.Bind<DamagingController>()
                .To<DamagingController>()
                .ToSingleton()
                .CrossContext();
            injectionBinder.Bind<LevelObjectType>()
                .ToValue(LevelObjectType.Player)
                .ToName("PlayerObjectType")
                .ToInject(false)
                .CrossContext();
            injectionBinder.Bind<string>()
                .ToValue("Player.Mesh")
                .ToName("PlayerMeshTag")
                .ToInject(false)
                .CrossContext();
        }

        void IModuleConfig.PostBindings(IInjectionBinder injectionBinder) {
            var eventBus = injectionBinder.GetInstance<IEventDispatcher>(
                ContextKeys.CROSS_CONTEXT_DISPATCHER);
            var smashableContoller =
                injectionBinder.GetInstance<SmashableController>();
            eventBus.AddListener(SmashableView.Event.TriggerEnter,
                smashableContoller.OnTriggerEnter);
            var damagingContoller =
                injectionBinder.GetInstance<DamagingController>();
            eventBus.AddListener(DamagingView.Event.TriggerEnter,
                damagingContoller.OnTriggerEnter);
        }
    }
}
