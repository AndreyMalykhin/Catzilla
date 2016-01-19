using UnityEngine;
using System.Collections;
using strange.extensions.injector.api;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.Config;
using Catzilla.LevelModule.View;
using Catzilla.LevelObjectModule.Model;
using Catzilla.LevelObjectModule.View;
using Catzilla.LevelObjectModule.Controller;

namespace Catzilla.LevelObjectModule.Config {
    public class LevelObjectModuleConfig: IModuleConfig {
        void IModuleConfig.InitBindings(IInjectionBinder injectionBinder) {
            injectionBinder.Bind<ObjectTypeInfoStorage>()
                .ToValue(Resources.Load<ObjectTypeInfoStorage>(
                    "ObjectTypeInfoStorage"))
                .ToInject(false)
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
            injectionBinder.Bind<ShootingController>()
                .To<ShootingController>()
                .ToSingleton()
                .CrossContext();
            injectionBinder.Bind<ScoreableController>()
                .To<ScoreableController>()
                .ToSingleton()
                .CrossContext();
            injectionBinder.Bind<ProjectileController>()
                .To<ProjectileController>()
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

            var scoreableContoller =
                injectionBinder.GetInstance<ScoreableController>();
            eventBus.AddListener(ScoreableView.Event.TriggerEnter,
                scoreableContoller.OnTriggerEnter);

            var shootingContoller =
                injectionBinder.GetInstance<ShootingController>();
            eventBus.AddListener(PlayerView.Event.Ready,
                shootingContoller.OnPlayerReady);
            eventBus.AddListener(ShootingView.Event.Ready,
                shootingContoller.OnViewReady);

            var playerController =
                injectionBinder.GetInstance<PlayerController>();
            eventBus.AddListener(PlayerView.Event.Death,
                playerController.OnDeath);
            eventBus.AddListener(PlayerView.Event.ScoreChange,
                playerController.OnScoreChange);
            eventBus.AddListener(LevelView.Event.Ready,
                playerController.OnLevelReady);
        }
    }
}
