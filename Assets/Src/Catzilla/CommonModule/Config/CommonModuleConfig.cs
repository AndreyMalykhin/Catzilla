﻿using UnityEngine;
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
    public class CommonModuleConfig: IModuleConfig {
        void IModuleConfig.InitBindings(IInjectionBinder injectionBinder) {
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
            injectionBinder.Bind<Server>()
                .To<Server>()
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
                .ToValue(Resources.Load<AudioManager>("AudioManager"))
                .ToInject(false)
                .ToSingleton()
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

        void IModuleConfig.PostBindings(IInjectionBinder injectionBinder) {
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
