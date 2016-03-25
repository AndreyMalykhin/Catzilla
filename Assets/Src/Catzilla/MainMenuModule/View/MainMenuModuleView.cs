﻿using UnityEngine;
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
            eventBus.On((int) Events.ServerDispose,
                mainScreenController.OnServerDispose);
            mainScreen.StartBtn.onClick.AddListener(
                mainScreenController.OnStartBtnClick);
            mainScreen.ExitBtn.onClick.AddListener(
                mainScreenController.OnExitBtnClick);
            mainScreen.LeaderboardBtn.onClick.AddListener(
                mainScreenController.OnLeaderboardBtnClick);
            mainScreen.AchievementsBtn.onClick.AddListener(
                mainScreenController.OnAchievementsBtnClick);
            mainScreen.FeedbackBtn.onClick.AddListener(
                mainScreenController.OnFeedbackBtnClick);
            mainScreen.ReplaysBtn.onClick.AddListener(
                mainScreenController.OnReplaysBtnClick);
            mainScreen.LoginBtn.onClick.AddListener(
                mainScreenController.OnLoginBtnClick);
        }
    }
}
