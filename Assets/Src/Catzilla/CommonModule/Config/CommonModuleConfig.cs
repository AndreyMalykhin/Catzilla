using UnityEngine;
using System.Collections;
using strange.extensions.injector.api;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using SmartLocalization;
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

        public override void InitBindings(IInjectionBinder injectionBinder) {
            if (Debug.isDebugBuild) {
                injectionBinder.Bind<Server>()
                    .ToValue(serverStub)
                    .CrossContext();
            } else {
                injectionBinder.Bind<Server>()
                    .ToValue(server)
                    .CrossContext();
            }

            injectionBinder.Bind<EventBus>()
                .To<EventBus>()
                .ToSingleton()
                .CrossContext();
            injectionBinder.Bind<Translator>()
                .To<Translator>()
                .ToSingleton()
                .CrossContext();
            injectionBinder.Bind<Game>()
                .To<Game>()
                .ToSingleton()
                .CrossContext();
            injectionBinder.Bind<PoolStorageController>()
                .To<PoolStorageController>()
                .ToSingleton()
                .CrossContext();
            injectionBinder.Bind<ActivityIndicatorController>()
                .To<ActivityIndicatorController>()
                .ToSingleton()
                .CrossContext();
            injectionBinder.Bind<Ad>()
                .To<Ad>()
                .ToSingleton()
                .CrossContext();
            injectionBinder.Bind<DisposableController>()
                .To<DisposableController>()
                .ToSingleton()
                .CrossContext();
            injectionBinder.Bind<BtnController>()
                .To<BtnController>()
                .ToSingleton()
                .CrossContext();
            var ui = GameObject.FindWithTag("UI").GetComponent<UIView>();
            injectionBinder.Bind<UIView>()
                .ToValue(ui)
                .ToInject(false)
                .CrossContext();
            var poolStorage = GameObject.FindWithTag("PoolStorage")
                .GetComponent<PoolStorageView>();
            injectionBinder.Bind<PoolStorageView>()
                .ToValue(poolStorage)
                .ToInject(false)
                .CrossContext();
            var coroutineManager = GameObject.FindWithTag("CoroutineManager")
                .GetComponent<CoroutineManagerView>();
            injectionBinder.Bind<CoroutineManagerView>()
                .ToValue(coroutineManager)
                .ToInject(false)
                .CrossContext();
            var activityIndicator = GameObject.FindWithTag("ActivityIndicator")
                .GetComponent<ActivityIndicatorView>();
            injectionBinder.Bind<ActivityIndicatorView>()
                .ToValue(activityIndicator)
                .ToInject(false)
                .CrossContext();
            injectionBinder.Bind<AudioManager>()
                .ToValue(audioManager)
                .CrossContext();
            injectionBinder.Bind<PopupManager>()
                .ToValue(popupManager)
                .CrossContext();
            injectionBinder.Bind<int>()
                .ToValue(0)
                .ToName("EffectsAudioChannel")
                .ToInject(false)
                .CrossContext();
            injectionBinder.Bind<int>()
                .ToValue(1)
                .ToName("UIAudioChannel")
                .ToInject(false)
                .CrossContext();
            injectionBinder.Bind<int>()
                .ToValue(2)
                .ToName("PlayerAudioChannel")
                .ToInject(false)
                .CrossContext();
        }

        public override void PostBindings(IInjectionBinder injectionBinder) {
            var eventBus = injectionBinder.GetInstance<EventBus>();

            var disposableController =
                injectionBinder.GetInstance<DisposableController>();
            eventBus.On(DisposableView.Event.TriggerExit,
                disposableController.OnTriggerExit);

            var poolStorageController =
                injectionBinder.GetInstance<PoolStorageController>();
            eventBus.On(
                Game.Event.LevelLoad, poolStorageController.OnLevelLoad);

            var btnController =
                injectionBinder.GetInstance<BtnController>();
            eventBus.On(
                BtnView.Event.Click, btnController.OnClick);

            var activityIndicatorController =
                injectionBinder.GetInstance<ActivityIndicatorController>();
            eventBus.On(Server.Event.Request,
                activityIndicatorController.OnServerRequest);
            eventBus.On(Server.Event.Response,
                activityIndicatorController.OnServerResponse);
        }
    }
}
