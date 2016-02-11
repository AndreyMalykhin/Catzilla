using UnityEngine;
using System.Collections;
using SmartLocalization;
using Zenject;
using Catzilla.CommonModule.View;
using Catzilla.CommonModule.Model;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.Controller;

namespace Catzilla.CommonModule.Config {
    [CreateAssetMenuAttribute]
    public class CommonModuleConfig: ModuleConfig {
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
            var ui = GameObject.FindWithTag("UI").GetComponent<UIView>();
            container.Bind<UIView>().ToInstance(ui);
            var poolStorage = GameObject.FindWithTag("PoolStorage")
                .GetComponent<PoolStorageView>();
            container.Bind<PoolStorageView>().ToInstance(poolStorage);
            var coroutineManager = GameObject.FindWithTag("CoroutineManager")
                .GetComponent<CoroutineManagerView>();
            container.Bind<CoroutineManagerView>().ToInstance(coroutineManager);
            var activityIndicator = GameObject.FindWithTag("ActivityIndicator")
                .GetComponent<ActivityIndicatorView>();
            container.Bind<ActivityIndicatorView>()
                .ToInstance(activityIndicator);
            container.Bind<Camera>("MainCamera").ToInstance(Camera.main);
            container.Bind<AudioManager>().ToInstance(audioManager);
            container.Bind<PopupManager>().ToInstance(popupManager);
            container.Bind<int>("EffectsHighPrioAudioChannel").ToInstance(0);
            container.Bind<int>("EffectsLowPrioAudioChannel").ToInstance(1);
            container.Bind<int>("UIAudioChannel").ToInstance(2);
            container.Bind<int>("PlayerAudioChannel").ToInstance(3);
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
