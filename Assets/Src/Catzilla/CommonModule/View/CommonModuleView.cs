using UnityEngine;
using System.Collections;
using SmartLocalization;
using Zenject;
using Catzilla.CommonModule.View;
using Catzilla.CommonModule.Model;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.Controller;

namespace Catzilla.CommonModule.View {
    public class CommonModuleView: ModuleView {
        [SerializeField]
        private Server server;

        [SerializeField]
        private Server serverStub;

        [SerializeField]
        private AudioManager audioManager;

        [SerializeField]
        private PopupManager popupManager;

        [SerializeField]
        private bool isServerStubbed = true;

        [SerializeField]
        private UIView ui;

        [SerializeField]
        private PoolStorageView poolStorage;

        [SerializeField]
        private CoroutineManagerView coroutineManager;

        [SerializeField]
        private ActivityIndicatorView activityIndicator;

        [SerializeField]
        private Camera mainCamera;

        public override void InitBindings(DiContainer container) {
            Server finalServer = server;

            if (Debug.isDebugBuild && isServerStubbed) {
                finalServer = serverStub;
            }

            container.Bind<Server>().ToInstance(finalServer);
            container.Bind<EventBus>().ToSingle();
            container.Bind<Translator>().ToSingle();
            container.Bind<Game>().ToSingle();
            container.Bind<PoolStorageController>().ToSingle();
            container.Bind<ActivityIndicatorController>().ToSingle();
            container.Bind<Ad>().ToSingle();
            container.Bind<DisposableController>().ToSingle();
            container.Bind<WorldSpacePopupController>().ToSingle();
            container.Bind<BtnController>().ToSingle();
            container.Bind<LeaderboardManager>().ToSingle();
            container.Bind<UIView>().ToInstance(ui);
            container.Bind<PoolStorageView>().ToInstance(poolStorage);
            container.Bind<CoroutineManagerView>().ToInstance(coroutineManager);
            container.Bind<ActivityIndicatorView>()
                .ToInstance(activityIndicator);
            container.Bind<Camera>("MainCamera").ToInstance(mainCamera);
            container.Bind<AudioManager>().ToInstance(audioManager);
            container.Bind<PopupManager>().ToInstance(popupManager);
            container.Bind<int>("EffectsHighPrioAudioChannel").ToInstance(0);
            container.Bind<int>("EffectsLowPrioAudioChannel").ToInstance(1);
            container.Bind<int>("UIHighPrioAudioChannel").ToInstance(2);
            container.Bind<int>("UILowPrioAudioChannel").ToInstance(3);
            container.Bind<int>("PlayerHighPrioAudioChannel").ToInstance(4);
            container.Bind<int>("PlayerLowPrioAudioChannel").ToInstance(5);
        }

        public override void PostBindings(DiContainer container) {
            container.Inject(container.Resolve<Server>());
            container.Inject(container.Resolve<AudioManager>());
            container.Inject(container.Resolve<PopupManager>());
            var eventBus = container.Resolve<EventBus>();

            var disposableController =
                container.Resolve<DisposableController>();
            eventBus.On(DisposableView.Event.TriggerExit,
                disposableController.OnTriggerExit);

            var poolStorageController =
                container.Resolve<PoolStorageController>();
            eventBus.On(
                Game.Event.LevelLoad, poolStorageController.OnLevelLoad);

            var btnController =
                container.Resolve<BtnController>();
            eventBus.On(
                BtnView.Event.Click, btnController.OnClick);

            var activityIndicatorController =
                container.Resolve<ActivityIndicatorController>();
            eventBus.On(Server.Event.Request,
                activityIndicatorController.OnServerRequest);
            eventBus.On(Server.Event.Response,
                activityIndicatorController.OnServerResponse);

            var worldSpacePopupController =
                container.Resolve<WorldSpacePopupController>();
            eventBus.On(WorldSpacePopupView.Event.Show,
                worldSpacePopupController.OnShow);
        }
    }
}
