using UnityEngine;
using System.Collections;
using strange.extensions.injector.api;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.Config;
using Catzilla.CommonModule.View;
using Catzilla.MainMenuModule.View;
using Catzilla.GameOverMenuModule.View;
using Catzilla.LevelObjectModule.View;
using Catzilla.AppModule.Controller;

namespace Catzilla.AppModule.Config {
    public class AppModuleConfig: IModuleConfig {
        void IModuleConfig.InitBindings(IInjectionBinder injectionBinder) {
            injectionBinder.Bind<AppController>()
                .To<AppController>()
                .ToSingleton()
                .CrossContext();
            injectionBinder.Bind<GameController>()
                .To<GameController>()
                .ToSingleton()
                .CrossContext();
            injectionBinder.Bind<Camera>()
                .ToValue(Camera.main)
                .ToName("MainCamera")
                .ToInject(false)
                .CrossContext();
        }

        void IModuleConfig.PostBindings(IInjectionBinder injectionBinder) {
            var contextEventBus = injectionBinder.GetInstance<IEventDispatcher>(
                ContextKeys.CONTEXT_DISPATCHER);
            var globalEventBus = injectionBinder.GetInstance<IEventDispatcher>(
                ContextKeys.CROSS_CONTEXT_DISPATCHER);

            var appController = injectionBinder.GetInstance<AppController>();
			contextEventBus.AddListener(
                ContextEvent.START, appController.OnStart);
            globalEventBus.AddListener(MainMenuView.Event.ExitBtnClick,
                appController.OnExit);
            globalEventBus.AddListener(GameOverMenuView.Event.ExitBtnClick,
                appController.OnExit);

            var gameController = injectionBinder.GetInstance<GameController>();
            globalEventBus.AddListener(MainMenuView.Event.StartBtnClick,
                gameController.OnStartBtnClick);
            globalEventBus.AddListener(GameOverMenuView.Event.RestartBtnClick,
                gameController.OnRestartBtnClick);
            globalEventBus.AddListener(PlayerView.Event.Death,
                gameController.OnPlayerDeath);
        }
    }
}
