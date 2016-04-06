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
            container.Bind<ShootingController>().ToSingle();
            container.Bind<ProjectileController>().ToSingle();
            container.Bind<ActivatableController>().ToSingle();
            container.Bind<TakeableController>().ToSingle();
            container.Bind<BonusController>().ToSingle();
            container.Bind<ExplosiveController>().ToSingle();
            container.Bind<DisposableController>().ToSingle();
            container.Bind<FleeingController>().ToSingle();
            container.Bind<ShockwavableController>().ToSingle();
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
            eventBus.On((int) Events.DisposableTriggerExit,
                disposableController.OnTriggerExit);

            var smashableContoller =
                container.Resolve<SmashableController>();
            eventBus.On((int) Events.SmashableSmash,
                smashableContoller.OnSmash);
            eventBus.On((int) Events.SmashableTriggerEnter,
                smashableContoller.OnTriggerEnter);
            // eventBus.On((int) Events.SmashableCollisionEnter,
            //     smashableContoller.OnCollisionEnter);
            eventBus.On((int) Events.ExplosiveExplode,
                smashableContoller.OnExplosion);

            var projectileContoller =
                container.Resolve<ProjectileController>();
            eventBus.On((int) Events.ProjectileTriggerEnter,
                projectileContoller.OnTriggerEnter);

            var shootingContoller =
                container.Resolve<ShootingController>();
            eventBus.On((int) Events.ShootingTriggerEnter,
                shootingContoller.OnTriggerEnter);
            eventBus.On((int) Events.ShootingShot,
                shootingContoller.OnShot);

            var takeableController = container.Resolve<TakeableController>();
            eventBus.On((int) Events.TakeableTriggerEnter,
                takeableController.OnTriggerEnter);

            var explosiveController = container.Resolve<ExplosiveController>();
            eventBus.On((int) Events.ExplosiveExplode,
                explosiveController.OnExplode);
            eventBus.On((int) Events.ExplosiveTriggerEnter,
                explosiveController.OnTriggerEnter);

            var bonusController = container.Resolve<BonusController>();
            eventBus.On((int)
                Events.BonusDestroy, bonusController.OnViewDestroy);

            var activatableContoller =
                container.Resolve<ActivatableController>();
            eventBus.On((int) Events.ActivatableTriggerEnter,
                activatableContoller.OnTriggerEnter);
            eventBus.On((int) Events.LevelGeneratorLevelGenerate,
                activatableContoller.OnLevelGenerate);

            var fleeingController = container.Resolve<FleeingController>();
            eventBus.On((int) Events.FleeingTriggerEnter,
                fleeingController.OnTriggerEnter);

            var shockwavableController =
                container.Resolve<ShockwavableController>();
            eventBus.On((int) Events.ExplosiveExplode,
                shockwavableController.OnExplosion);
            eventBus.On((int) Events.ShockwavableTriggerEnter,
                shockwavableController.OnTriggerEnter);
            eventBus.On((int) Events.ShootingShot,
                shockwavableController.OnShot);

            var playerController =
                container.Resolve<PlayerController>();
            eventBus.On((int) Events.PlayerConstruct,
                playerController.OnViewConstruct);
            eventBus.On((int) Events.PlayerDestroy,
                playerController.OnViewDestroy);
            eventBus.On((int) Events.PlayerTriggerEnter,
                playerController.OnTriggerEnter);
            eventBus.On((int) Events.PlayerDeath,
                playerController.OnDeath);
            eventBus.On((int) Events.PlayerScoreChange,
                playerController.OnScoreChange);
            eventBus.On((int) Events.PlayerHealthChange,
                playerController.OnHealthChange);
            eventBus.On((int) Events.PlayerResurrect,
                playerController.OnResurrect);
            eventBus.On((int) Events.PlayerFootstep,
                playerController.OnFootstep);
            eventBus.On((int) Events.PlayerRefuse,
                playerController.OnRefuse);
            eventBus.On((int) Events.PlayerSmashStreak,
                playerController.OnSmashStreak);
            eventBus.On((int) Events.ShockwavablePropagate,
                playerController.OnShockwave);
        }
    }
}
