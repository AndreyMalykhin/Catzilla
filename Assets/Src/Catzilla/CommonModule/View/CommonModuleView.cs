using UnityEngine;
using System.Collections;
using System.Diagnostics;
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
        private WorldSpacePopupManager worldSpacePopupManager;

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

        [SerializeField]
        private ScreenSpacePopupManagerView screenSpacePopupManager;

        [SerializeField]
        private UIBlockerView uiBlocker;

        [SerializeField]
        private string emptyScene;

        public override void InitBindings(DiContainer container) {
            Server finalServer = server;

            if (UnityEngine.Debug.isDebugBuild && isServerStubbed) {
                finalServer = serverStub;
            }

            container.Bind<Server>().ToInstance(finalServer);
            container.Bind<AuthManager>().ToSingle();
            container.Bind<EventBus>().ToSingle();
            container.Bind<Translator>().ToSingle();
            container.Bind<Game>().ToSingle();
            container.Bind<PoolStorageController>().ToSingle();
            container.Bind<ActivityIndicatorController>().ToSingle();
            container.Bind<Ad>().ToSingle();
            container.Bind<DisposableController>().ToSingle();
            container.Bind<BtnController>().ToSingle();
            container.Bind<UIView>().ToInstance(ui);
            container.Bind<PoolStorageView>().ToInstance(poolStorage);
            container.Bind<CoroutineManagerView>().ToInstance(coroutineManager);
            container.Bind<ScreenSpacePopupManagerView>()
                .ToInstance(screenSpacePopupManager);
            container.Bind<ActivityIndicatorView>()
                .ToInstance(activityIndicator);
            container.Bind<Camera>("MainCamera").ToInstance(mainCamera);
            container.Bind<AudioManager>().ToInstance(audioManager);
            container.Bind<UIBlockerView>().ToInstance(uiBlocker);
            container.Bind<WorldSpacePopupManager>()
                .ToInstance(worldSpacePopupManager);
            container.Bind<Stopwatch>("PlayStopwatch")
                .ToInstance(new Stopwatch());
            container.Bind<string>("EmptyScene").ToInstance(emptyScene);
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
            container.Inject(container.Resolve<WorldSpacePopupManager>());
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
        }
    }
}
