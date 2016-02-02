using UnityEngine;
using System.Collections;
using strange.extensions.injector.api;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.Config;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.Model;
using Catzilla.MainMenuModule.Controller;
using Catzilla.MainMenuModule.View;

namespace Catzilla.MainMenuModule.Config {
    public class MainMenuModuleConfig: IModuleConfig {
        void IModuleConfig.InitBindings(IInjectionBinder injectionBinder) {
            var screen = GameObject.FindWithTag("MainScreen")
                .GetComponent<MainScreenView>();
            injectionBinder.Bind<MainScreenView>()
                .ToValue(screen)
                .ToInject(false)
                .CrossContext();
            var menu = GameObject.FindWithTag("MainScreen.Menu")
                .GetComponent<MainMenuView>();
            injectionBinder.Bind<MainMenuView>()
                .ToValue(menu)
                .ToInject(false)
                .CrossContext();
            injectionBinder.Bind<MainMenuController>()
                .To<MainMenuController>()
                .ToSingleton()
                .CrossContext();
        }

        void IModuleConfig.PostBindings(IInjectionBinder injectionBinder) {
            var eventBus = injectionBinder.GetInstance<EventBus>();
            var mainMenuController =
                injectionBinder.GetInstance<MainMenuController>();
            eventBus.On(MainMenuView.Event.ExitBtnClick,
                mainMenuController.OnExitBtnClick);
            eventBus.On(MainMenuView.Event.StartBtnClick,
                mainMenuController.OnStartBtnClick);
            eventBus.On(MainMenuView.Event.LeaderboardBtnClick,
                mainMenuController.OnLeaderboardBtnClick);
            eventBus.On(Server.Event.Dispose,
                mainMenuController.OnServerDispose);
        }
    }
}
