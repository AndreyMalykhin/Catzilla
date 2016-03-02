using UnityEngine;
using System.Collections;
using Zenject;
using SmartLocalization;
using Catzilla.CommonModule.View;
using Catzilla.CommonModule.Util;
using Catzilla.LevelObjectModule.View;
using Catzilla.LevelModule.View;
using Catzilla.PlayerModule.Model;
using Catzilla.PlayerModule.View;
using Catzilla.PlayerModule.Controller;

namespace Catzilla.PlayerModule.View {
    public class PlayerModuleView: ModuleView {
        [SerializeField]
        private PlayerStateStorage playerStateStorage;

        [SerializeField]
        private PlayerStateStorage playerStateStorageStub;

        [SerializeField]
        private bool isPlayerStateStorageStubbed = true;

        [SerializeField]
        private GiftManager giftManager;

        public override void InitBindings(DiContainer container) {
            PlayerStateStorage finalPlayerStateStorage = playerStateStorage;

            if (Debug.isDebugBuild && isPlayerStateStorageStubbed) {
                finalPlayerStateStorage = playerStateStorageStub;
            }

            container.Bind<PlayerStateStorage>()
                .ToInstance(finalPlayerStateStorage);
            container.Bind<GiftManager>().ToInstance(giftManager);
            container.Bind<HUDScoreController>().ToSingle();
            container.Bind<HUDHealthController>().ToSingle();
            container.Bind<PlayerManager>().ToSingle();
        }

        public override void PostBindings(DiContainer container) {
            container.Inject(container.Resolve<PlayerStateStorage>());
            container.Inject(container.Resolve<GiftManager>());
            var eventBus = container.Resolve<EventBus>();

            var playerScoreController =
                container.Resolve<HUDScoreController>();
            eventBus.On(
                PlayerView.Event.ScoreChange, playerScoreController.OnChange);
            eventBus.On(HUDScoreView.Event.Construct,
                playerScoreController.OnViewConstruct);

            var playerHealthController =
                container.Resolve<HUDHealthController>();
            eventBus.On(PlayerView.Event.HealthChange,
                playerHealthController.OnChange);
            eventBus.On(PlayerView.Event.Construct,
                playerHealthController.OnPlayerConstruct);
        }
    }
}
