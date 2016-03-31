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

        [SerializeField]
        private RewardManager rewardManager;

        [SerializeField]
        private GameObject hudWrapper;

        [SerializeField]
        private int scoreWorldPopupType;

        [SerializeField]
        private int resurrectionWorldPopupType;

        [SerializeField]
        private int rewardWorldPopupType;

        [SerializeField]
        private int speechWorldPopupType;

        [SerializeField]
        private int smashStreakPopupType;

        public override void InitBindings(DiContainer container) {
            PlayerStateStorage finalPlayerStateStorage = playerStateStorage;

            if (Debug.isDebugBuild && isPlayerStateStorageStubbed) {
                finalPlayerStateStorage = playerStateStorageStub;
            }

            container.Bind<PlayerStateStorage>()
                .ToInstance(finalPlayerStateStorage);
            container.Bind<GiftManager>().ToInstance(giftManager);
            container.Bind<RewardManager>().ToInstance(rewardManager);
            container.Bind<GameObject>("HUDWrapper").ToInstance(hudWrapper);
            container.Bind<int>("ScoreWorldPopupType")
                .ToInstance(scoreWorldPopupType);
            container.Bind<int>("ResurrectionWorldPopupType")
                .ToInstance(resurrectionWorldPopupType);
            container.Bind<int>("RewardWorldPopupType")
                .ToInstance(rewardWorldPopupType);
            container.Bind<int>("SpeechWorldPopupType")
                .ToInstance(speechWorldPopupType);
            container.Bind<int>("SmashStreakPopupType")
                .ToInstance(smashStreakPopupType);
            container.Bind<HUDScoreController>().ToSingle();
            container.Bind<HUDHealthController>().ToSingle();
            container.Bind<HUDNotificationsController>().ToSingle();
            container.Bind<PlayerManager>().ToSingle();
        }

        public override void PostBindings(DiContainer container) {
            container.Inject(container.Resolve<PlayerStateStorage>());
            container.Inject(container.Resolve<GiftManager>());
            container.Inject(container.Resolve<RewardManager>());
            var eventBus = container.Resolve<EventBus>();

            var hudScoreController =
                container.Resolve<HUDScoreController>();
            eventBus.On((int)
                Events.PlayerScoreChange, hudScoreController.OnChange);
            eventBus.On((int) Events.HUDScoreConstruct,
                hudScoreController.OnViewConstruct);

            var hudHealthController =
                container.Resolve<HUDHealthController>();
            eventBus.On((int) Events.PlayerHealthChange,
                hudHealthController.OnChange);
            eventBus.On((int) Events.PlayerConstruct,
                hudHealthController.OnPlayerConstruct);

            var hudNotificationsController =
                container.Resolve<HUDNotificationsController>();
            eventBus.On((int) Events.HUDNotificationsConstruct,
                hudNotificationsController.OnConstruct);
            eventBus.On((int) Events.PlayerSmashStreak,
                hudNotificationsController.OnSmashStreak);
        }
    }
}
