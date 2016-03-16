using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.Text;
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

        [SerializeField]
        private string feedbackEmail;

        [SerializeField]
        private string secretKey;

        [SerializeField]
        private string errorMsg;

        [SerializeField]
        private int errorPopupType;

        [SerializeField]
        private int commonPopupType;

        [SerializeField]
        private int effectsHighPrioAudioChannel;

        [SerializeField]
        private int effectsLowPrioAudioChannel;

        [SerializeField]
        private int uiHighPrioAudioChannel;

        [SerializeField]
        private int uiLowPrioAudioChannel;

        [SerializeField]
        private int playerHighPrioAudioChannel;

        [SerializeField]
        private int playerLowPrioAudioChannel;

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
            container.Bind<ShowableController>().ToSingle();
            container.Bind<ErrorController>().ToSingle();
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
            container.Bind<string>("FeedbackEmail").ToInstance(feedbackEmail);
            container.Bind<string>("ErrorMsg").ToInstance(errorMsg);
            container.Bind<int>("ErrorPopupType").ToInstance(errorPopupType);
            container.Bind<int>("CommonPopupType").ToInstance(commonPopupType);
            container.Bind<byte[]>("SecretKey")
                .ToInstance(Encoding.UTF8.GetBytes(secretKey));
            container.Bind<int>("EffectsHighPrioAudioChannel")
                .ToInstance(effectsHighPrioAudioChannel);
            container.Bind<int>("EffectsLowPrioAudioChannel")
                .ToInstance(effectsLowPrioAudioChannel);
            container.Bind<int>("UIHighPrioAudioChannel")
                .ToInstance(uiHighPrioAudioChannel);
            container.Bind<int>("UILowPrioAudioChannel")
                .ToInstance(uiLowPrioAudioChannel);
            container.Bind<int>("PlayerHighPrioAudioChannel")
                .ToInstance(playerHighPrioAudioChannel);
            container.Bind<int>("PlayerLowPrioAudioChannel")
                .ToInstance(playerLowPrioAudioChannel);
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

            var showableController =
                container.Resolve<ShowableController>();
            eventBus.On(
                ShowableView.Event.Show, showableController.OnShow);

            var activityIndicatorController =
                container.Resolve<ActivityIndicatorController>();
            eventBus.On(Server.Event.Request,
                activityIndicatorController.OnServerRequest);
            eventBus.On(Server.Event.Response,
                activityIndicatorController.OnServerResponse);

            var errorController =
                container.Resolve<ErrorController>();
            eventBus.On(
                Server.Event.Response, errorController.OnServerResponse);
        }
    }
}
