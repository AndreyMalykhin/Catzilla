using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.View;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.Model;
using Catzilla.MainMenuModule.Controller;
using Catzilla.MainMenuModule.View;

namespace Catzilla.MainMenuModule.View {
    public class MainMenuModuleView: ModuleView {
        [SerializeField]
        private MainScreenView screen;

        [SerializeField]
        private MainMenuView menu;

        public override void InitBindings(DiContainer container) {
            container.Bind<MainScreenView>().ToInstance(screen);
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
