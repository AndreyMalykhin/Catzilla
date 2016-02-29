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

        public override void InitBindings(DiContainer container) {
            container.Bind<MainScreenView>().ToInstance(screen);
            container.Bind<MainScreenController>().ToSingle();
        }

        public override void PostBindings(DiContainer container) {
            var eventBus = container.Resolve<EventBus>();
            var mainScreenController =
                container.Resolve<MainScreenController>();
            eventBus.On(MainMenuView.Event.ExitBtnClick,
                mainScreenController.OnExitBtnClick);
            eventBus.On(MainMenuView.Event.StartBtnClick,
                mainScreenController.OnStartBtnClick);
            eventBus.On(MainMenuView.Event.LeaderboardBtnClick,
                mainScreenController.OnLeaderboardBtnClick);
            eventBus.On(MainMenuView.Event.AchievementsBtnClick,
                mainScreenController.OnAchievementsBtnClick);
            eventBus.On(Server.Event.Dispose,
                mainScreenController.OnServerDispose);
            var mainScreen = container.Resolve<MainScreenView>();
            mainScreen.LoginBtn.onClick.AddListener(
                mainScreenController.OnLoginBtnClick);
        }
    }
}
