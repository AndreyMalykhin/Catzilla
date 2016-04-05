using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.Text;
using SmartLocalization;
using Zenject;
using Catzilla.LevelModule.View;
using Catzilla.LevelObjectModule.View;
using Catzilla.GameOverMenuModule.View;
using Catzilla.MainMenuModule.View;
using Catzilla.PlayerModule.Model;
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
        private WorldSpacePopupManagerView worldSpacePopupManager;

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
        private TutorialScreenView tutorialScreen;

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
            container.Bind<BtnController>().ToSingle();
            container.Bind<ShowableController>().ToSingle();
            container.Bind<ErrorController>().ToSingle();
            container.Bind<GameController>().ToSingle();
            container.Bind<EveryplayController>().ToSingle();
            container.Bind<AnalyticsController>().ToSingle();
            container.Bind<PoolStorageView>().ToInstance(poolStorage);
            container.Bind<CoroutineManagerView>().ToInstance(coroutineManager);
            container.Bind<ScreenSpacePopupManagerView>()
                .ToInstance(screenSpacePopupManager);
            container.Bind<ActivityIndicatorView>()
                .ToInstance(activityIndicator);
            container.Bind<Camera>("MainCamera").ToInstance(mainCamera);
            container.Bind<AudioManager>().ToInstance(audioManager);
            container.Bind<UIBlockerView>().ToInstance(uiBlocker);
            container.Bind<WorldSpacePopupManagerView>()
                .ToInstance(worldSpacePopupManager);
            container.Bind<Stopwatch>("PlayStopwatch")
                .ToInstance(new Stopwatch());
            container.Bind<TutorialScreenView>().ToInstance(tutorialScreen);
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
            var eventBus = container.Resolve<EventBus>();

            var poolStorageController =
                container.Resolve<PoolStorageController>();
            eventBus.On((int)
                Events.GameLevelLoad, poolStorageController.OnLevelLoad);

            var btnController =
                container.Resolve<BtnController>();
            eventBus.On((int)
                Events.BtnClick, btnController.OnClick);

            var showableController =
                container.Resolve<ShowableController>();
            eventBus.On((int)
                Events.ShowableShow, showableController.OnShow);
            eventBus.On((int)
                Events.ShowablePreShow, showableController.OnPreShow);

            var activityIndicatorController =
                container.Resolve<ActivityIndicatorController>();
            eventBus.On((int) Events.ServerRequest,
                activityIndicatorController.OnServerRequest);
            eventBus.On((int) Events.ServerResponse,
                activityIndicatorController.OnServerResponse);

            var errorController =
                container.Resolve<ErrorController>();
            eventBus.On((int)
                Events.ServerResponse, errorController.OnServerResponse);

            var gameController = container.Resolve<GameController>();
            eventBus.On((int) Events.LevelGeneratorLevelGenerate,
                gameController.OnLevelGenerate);
            var levelStartScreen = container.Resolve<LevelStartScreenView>()
                .GetComponent<ShowableView>();
            levelStartScreen.OnHide += gameController.OnLevelStartScreenHide;
            var gameOverScreen = container.Resolve<GameOverScreenView>();
            var gameOverScreenShowable =
                gameOverScreen.GetComponent<ShowableView>();
            gameOverScreenShowable.OnShow +=
                gameController.OnGameOverScreenShow;
            gameOverScreenShowable.OnHide +=
                gameController.OnGameOverScreenHide;
            var tutorialScreen = container.Resolve<TutorialScreenView>()
                .GetComponent<ShowableView>();
            tutorialScreen.OnPreShow += gameController.OnTutorialPreShow;
            tutorialScreen.OnHide += gameController.OnTutorialHide;

            var everyplayController = container.Resolve<EveryplayController>();
            eventBus.On((int)
                Events.GameLevelUnload, everyplayController.OnLevelUnload);
            eventBus.On((int)
                Events.GamePreLevelLoad, everyplayController.OnPreLevelLoad);
            eventBus.On((int) Events.PlayerResurrect,
                everyplayController.OnPlayerResurrect);
            eventBus.On((int) Events.PlayerManagerPreLevelComplete,
                everyplayController.OnPreLevelComplete);
            gameOverScreenShowable.OnShow +=
                everyplayController.OnGameOverScreenShow;
            levelStartScreen.OnShow +=
                everyplayController.OnLevelStartScreenShow;
            var levelCompleteScreen =
                container.Resolve<LevelCompleteScreenView>();
            levelCompleteScreen.GetComponent<ShowableView>().OnShow +=
                everyplayController.OnLevelCompleteScreenShow;

            var analyticsController = container.Resolve<AnalyticsController>();
            eventBus.On((int) Events.PlayerManagerPreLevelComplete,
                analyticsController.OnPreLevelComplete);
            eventBus.On((int) Events.PlayerDeath,
                analyticsController.OnPlayerDeath);
            eventBus.On((int) Events.PlayerResurrect,
                analyticsController.OnPlayerResurrect);
            eventBus.On((int) Events.PlayerConstruct,
                analyticsController.OnPlayerConstruct);
            eventBus.On((int) Events.AppStart,
                analyticsController.OnAppStart);
            var mainScreen = container.Resolve<MainScreenView>();
            mainScreen.FeedbackBtn.onClick.AddListener(
                analyticsController.OnMainScreenFeedbackBtnClick);
            mainScreen.StartBtn.onClick.AddListener(
                analyticsController.OnMainScreenStartBtnClick);
            mainScreen.LeaderboardBtn.onClick.AddListener(
                analyticsController.OnMainScreenLeaderboardBtnClick);
            mainScreen.AchievementsBtn.onClick.AddListener(
                analyticsController.OnMainScreenAchievementsBtnClick);
            mainScreen.ReplaysBtn.onClick.AddListener(
                analyticsController.OnMainScreenReplaysBtnClick);
            mainScreen.ExitBtn.onClick.AddListener(
                analyticsController.OnMainScreenExitBtnClick);
            gameOverScreen.ExitBtn.onClick.AddListener(
                analyticsController.OnGameOverScreenExitBtnClick);
            gameOverScreen.RestartBtn.onClick.AddListener(
                analyticsController.OnGameOverScreenRestartBtnClick);
            levelCompleteScreen.ShareBtn.onClick.AddListener(
                analyticsController.OnLevelCompleteScreenShareBtnClick);
            var ad = container.Resolve<Ad>();
            ad.OnView += analyticsController.OnAdView;
            var authManager = container.Resolve<AuthManager>();
            authManager.OnLoginSuccess += analyticsController.OnLogin;
        }
    }
}
