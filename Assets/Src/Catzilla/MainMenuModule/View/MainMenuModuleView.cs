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
            var mainScreen = container.Resolve<MainScreenView>();
            eventBus.On(Server.Event.Dispose,
                mainScreenController.OnServerDispose);
            mainScreen.Menu.StartBtn.onClick.AddListener(
                mainScreenController.OnStartBtnClick);
            mainScreen.Menu.ExitBtn.onClick.AddListener(
                mainScreenController.OnExitBtnClick);
            mainScreen.Menu.LeaderboardBtn.onClick.AddListener(
                mainScreenController.OnLeaderboardBtnClick);
            mainScreen.Menu.AchievementsBtn.onClick.AddListener(
                mainScreenController.OnAchievementsBtnClick);
            mainScreen.Menu.FeedbackBtn.onClick.AddListener(
                mainScreenController.OnFeedbackBtnClick);
            mainScreen.LoginBtn.onClick.AddListener(
                mainScreenController.OnLoginBtnClick);
        }
    }
}
