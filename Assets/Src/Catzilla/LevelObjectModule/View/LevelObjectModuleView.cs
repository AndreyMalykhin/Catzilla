using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;
using Catzilla.LevelModule.View;
using Catzilla.LevelObjectModule.Model;
using Catzilla.LevelObjectModule.View;
using Catzilla.LevelObjectModule.Controller;

namespace Catzilla.LevelObjectModule.View {
    public class LevelObjectModuleView: ModuleView {
        [SerializeField]
        private ObjectTypeInfoStorage objectTypeInfoStorage;

        [SerializeField]
        private string playerMeshTag;

        [SerializeField]
        private string playerTag;

        [SerializeField]
        private string playerFieldOfViewTag;

        public override void InitBindings(DiContainer container) {
            container.Bind<ObjectTypeInfoStorage>()
                .ToInstance(objectTypeInfoStorage);
            container.Bind<PlayerController>().ToSingle();
            container.Bind<SmashableController>().ToSingle();
            container.Bind<SmashingController>().ToSingle();
            container.Bind<DamagingController>().ToSingle();
            container.Bind<ShootingController>().ToSingle();
            container.Bind<ProjectileController>().ToSingle();
            container.Bind<ActivatableController>().ToSingle();
            container.Bind<TakeableController>().ToSingle();
            container.Bind<BonusController>().ToSingle();
            container.Bind<ExplosiveController>().ToSingle();
            container.Bind<DisposableController>().ToSingle();
            container.Bind<LevelObjectType>("PlayerObjectType")
                .ToInstance(LevelObjectType.Player);
            container.Bind<string>("PlayerTag").ToInstance(playerTag);
            container.Bind<string>("PlayerMeshTag").ToInstance(playerMeshTag);
            container.Bind<string>("PlayerFieldOfViewTag")
                .ToInstance(playerFieldOfViewTag);
        }

        public override void PostBindings(DiContainer container) {
            container.Inject(container.Resolve<ObjectTypeInfoStorage>());
            var eventBus = container.Resolve<EventBus>();

            var disposableController =
                container.Resolve<DisposableController>();
            eventBus.On(DisposableView.Event.TriggerExit,
                disposableController.OnTriggerExit);

            var smashableContoller =
                container.Resolve<SmashableController>();
            eventBus.On(SmashableView.Event.Smash,
                smashableContoller.OnSmash);

            var smashingContoller =
                container.Resolve<SmashingController>();
            eventBus.On(SmashingView.Event.TriggerEnter,
                smashingContoller.OnTriggerEnter);

            var damagingContoller =
                container.Resolve<DamagingController>();
            eventBus.On(DamagingView.Event.TriggerEnter,
                damagingContoller.OnTriggerEnter);

            var projectileContoller =
                container.Resolve<ProjectileController>();
            eventBus.On(ProjectileView.Event.TriggerEnter,
                projectileContoller.OnTriggerEnter);

            var shootingContoller =
                container.Resolve<ShootingController>();
            eventBus.On(ShootingView.Event.TriggerEnter,
                shootingContoller.OnTriggerEnter);
            eventBus.On(ShootingView.Event.Shot,
                shootingContoller.OnShot);

            var takeableController = container.Resolve<TakeableController>();
            eventBus.On(TakeableView.Event.TriggerEnter,
                takeableController.OnTriggerEnter);

            var explosiveController = container.Resolve<ExplosiveController>();
            eventBus.On(ExplosiveView.Event.Explode,
                explosiveController.OnExplode);

            var bonusController = container.Resolve<BonusController>();
            eventBus.On(
                BonusView.Event.Destroy, bonusController.OnViewDestroy);

            var activatableContoller =
                container.Resolve<ActivatableController>();
            eventBus.On(ActivatableView.Event.TriggerEnter,
                activatableContoller.OnTriggerEnter);

            var playerController =
                container.Resolve<PlayerController>();
            eventBus.On(PlayerView.Event.Construct,
                playerController.OnViewConstruct);
            eventBus.On(PlayerView.Event.Destroy,
                playerController.OnViewDestroy);
            eventBus.On(PlayerView.Event.Death,
                playerController.OnDeath);
            eventBus.On(PlayerView.Event.ScoreChange,
                playerController.OnScoreChange);
            eventBus.On(PlayerView.Event.HealthChange,
                playerController.OnHealthChange);
            eventBus.On(PlayerView.Event.Resurrect,
                playerController.OnResurrect);
            eventBus.On(PlayerView.Event.Footstep,
                playerController.OnFootstep);
        }
    }
}
