using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Config;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.Model;
using Catzilla.MainMenuModule.Controller;
using Catzilla.MainMenuModule.View;

namespace Catzilla.MainMenuModule.Config {
    [CreateAssetMenuAttribute]
    public class MainMenuModuleConfig: ModuleConfig {
        public override void InitBindings(DiContainer container) {
            var screen = GameObject.FindWithTag("MainScreen")
                .GetComponent<MainScreenView>();
            container.Bind<MainScreenView>().ToInstance(screen);
            var menu = GameObject.FindWithTag("MainScreen.Menu")
                .GetComponent<MainMenuView>();
            container.Bind<MainMenuView>().ToInstance(menu);
            container.Bind<MainMenuController>().ToSingle();
        }

        public override void PostBindings(DiContainer container) {
            var eventBus = container.Resolve<EventBus>();
            var mainMenuController =
                container.Resolve<MainMenuController>();
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
