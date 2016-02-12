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
        private WorldSpacePopupView scorePopupProto;

        [SerializeField]
        private string playerMeshTag;

        [SerializeField]
        private string playerFieldOfViewTag;

        [SerializeField]
        private string projectileTag;

        public override void InitBindings(DiContainer container) {
            container.Bind<ObjectTypeInfoStorage>()
                .ToInstance(objectTypeInfoStorage);
            container.Bind<PlayerController>().ToSingle();
            container.Bind<SmashableController>().ToSingle();
            container.Bind<DamagingController>().ToSingle();
            container.Bind<ShootingController>().ToSingle();
            container.Bind<ScoreableController>().ToSingle();
            container.Bind<ProjectileController>().ToSingle();
            container.Bind<ActivatableController>().ToSingle();
            container.Bind<WorldSpacePopupView>("ScorePopupProto")
                .ToInstance(scorePopupProto);
            container.Bind<LevelObjectType>("PlayerObjectType")
                .ToInstance(LevelObjectType.Player);
            container.Bind<string>("PlayerMeshTag").ToInstance(playerMeshTag);
            container.Bind<string>("PlayerFieldOfViewTag")
                .ToInstance(playerFieldOfViewTag);
            container.Bind<string>("ProjectileTag").ToInstance(projectileTag);
        }

        public override void PostBindings(DiContainer container) {
            container.Inject(container.Resolve<ObjectTypeInfoStorage>());
            var eventBus = container.Resolve<EventBus>();

            var smashableContoller =
                container.Resolve<SmashableController>();
            eventBus.On(SmashableView.Event.TriggerEnter,
                smashableContoller.OnTriggerEnter);

            var damagingContoller =
                container.Resolve<DamagingController>();
            eventBus.On(DamagingView.Event.TriggerEnter,
                damagingContoller.OnTriggerEnter);

            var scoreableContoller =
                container.Resolve<ScoreableController>();
            eventBus.On(ScoreableView.Event.TriggerEnter,
                scoreableContoller.OnTriggerEnter);

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

            var activatableContoller =
                container.Resolve<ActivatableController>();
            eventBus.On(ActivatableView.Event.TriggerEnter,
                activatableContoller.OnTriggerEnter);

            var playerController =
                container.Resolve<PlayerController>();
            eventBus.On(PlayerView.Event.Death,
                playerController.OnDeath);
            eventBus.On(PlayerView.Event.ScoreChange,
                playerController.OnScoreChange);
            eventBus.On(PlayerView.Event.Resurrect,
                playerController.OnResurrect);
            eventBus.On(PlayerView.Event.Footstep,
                playerController.OnFootstep);
        }
    }
}
