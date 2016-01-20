﻿using UnityEngine;
using System.Collections;
using strange.extensions.injector.api;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.Config;
using Catzilla.LevelObjectModule.View;
using Catzilla.GameOverMenuModule.Controller;
using Catzilla.GameOverMenuModule.View;

namespace Catzilla.GameOverMenuModule.Config {
    public class GameOverMenuModuleConfig: IModuleConfig {
        void IModuleConfig.InitBindings(IInjectionBinder injectionBinder) {
            var screen = GameObject.FindWithTag("GameOverScreen")
                .GetComponent<GameOverScreenView>();
            injectionBinder.Bind<GameOverScreenView>()
                .ToValue(screen)
                .ToInject(false)
                .CrossContext();
            var menu = GameObject.FindWithTag("GameOverScreen.Menu")
                .GetComponent<GameOverMenuView>();
            injectionBinder.Bind<GameOverMenuView>()
                .ToValue(menu)
                .ToInject(false)
                .CrossContext();
            injectionBinder.Bind<GameOverMenuController>()
                .To<GameOverMenuController>()
                .ToSingleton()
                .CrossContext();
        }

        void IModuleConfig.PostBindings(IInjectionBinder injectionBinder) {
            var eventBus = injectionBinder.GetInstance<IEventDispatcher>(
                ContextKeys.CROSS_CONTEXT_DISPATCHER);
            var gameOverMenuController =
                injectionBinder.GetInstance<GameOverMenuController>();
            eventBus.AddListener(GameOverMenuView.Event.ExitBtnClick,
                gameOverMenuController.OnExitBtnClick);
            eventBus.AddListener(GameOverMenuView.Event.RestartBtnClick,
                gameOverMenuController.OnRestartBtnClick);
            eventBus.AddListener(GameOverMenuView.Event.ResurrectBtnClick,
                gameOverMenuController.OnResurrectBtnClick);
            eventBus.AddListener(PlayerView.Event.Construct,
                gameOverMenuController.OnPlayerConstruct);
        }
    }
}
