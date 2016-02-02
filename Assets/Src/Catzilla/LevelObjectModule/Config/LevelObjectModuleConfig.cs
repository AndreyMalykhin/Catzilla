using UnityEngine;
using System.Collections;
using strange.extensions.injector.api;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.Config;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;
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
            injectionBinder.Bind<ActivatableController>()
                .To<ActivatableController>()
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
            injectionBinder.Bind<string>()
                .ToValue("Player.FieldOfView")
                .ToName("PlayerFieldOfViewTag")
                .ToInject(false)
                .CrossContext();
            injectionBinder.Bind<string>()
                .ToValue("Projectile")
                .ToName("ProjectileTag")
                .ToInject(false)
                .CrossContext();
        }

        void IModuleConfig.PostBindings(IInjectionBinder injectionBinder) {
            var eventBus = injectionBinder.GetInstance<EventBus>();

            var smashableContoller =
                injectionBinder.GetInstance<SmashableController>();
            eventBus.On(SmashableView.Event.TriggerEnter,
                smashableContoller.OnTriggerEnter);

            var damagingContoller =
                injectionBinder.GetInstance<DamagingController>();
            eventBus.On(DamagingView.Event.TriggerEnter,
                damagingContoller.OnTriggerEnter);

            var scoreableContoller =
                injectionBinder.GetInstance<ScoreableController>();
            eventBus.On(ScoreableView.Event.TriggerEnter,
                scoreableContoller.OnTriggerEnter);

            var projectileContoller =
                injectionBinder.GetInstance<ProjectileController>();
            eventBus.On(ProjectileView.Event.TriggerEnter,
                projectileContoller.OnTriggerEnter);

            var shootingContoller =
                injectionBinder.GetInstance<ShootingController>();
            eventBus.On(ShootingView.Event.TriggerEnter,
                shootingContoller.OnTriggerEnter);
            eventBus.On(ShootingView.Event.Shot,
                shootingContoller.OnShot);

            var activatableContoller =
                injectionBinder.GetInstance<ActivatableController>();
            eventBus.On(ActivatableView.Event.TriggerEnter,
                activatableContoller.OnTriggerEnter);

            var playerController =
                injectionBinder.GetInstance<PlayerController>();
            eventBus.On(PlayerView.Event.Death,
                playerController.OnDeath);
            eventBus.On(PlayerView.Event.ScoreChange,
                playerController.OnScoreChange);
            eventBus.On(LevelView.Event.Construct,
                playerController.OnLevelConstruct);
            eventBus.On(PlayerView.Event.Resurrect,
                playerController.OnResurrect);
        }
    }
}
