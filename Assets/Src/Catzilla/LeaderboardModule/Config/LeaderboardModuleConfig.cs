using UnityEngine;
using System.Collections;
using strange.extensions.injector.api;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.Config;
using Catzilla.CommonModule.Util;
using Catzilla.LeaderboardModule.View;
using Catzilla.LeaderboardModule.Model;
using Catzilla.LeaderboardModule.Controller;

namespace Catzilla.LeaderboardModule.Config {
    public class LeaderboardModuleConfig: IModuleConfig {
        void IModuleConfig.InitBindings(IInjectionBinder injectionBinder) {
            var leaderboardsScreen = GameObject.FindWithTag(
                "LeaderboardsScreen").GetComponent<LeaderboardsScreenView>();
            injectionBinder.Bind<LeaderboardsScreenView>()
                .ToValue(leaderboardsScreen)
                .ToInject(false)
                .CrossContext();
            injectionBinder.Bind<LeaderboardsScreenController>()
                .To<LeaderboardsScreenController>()
                .ToSingleton()
                .CrossContext();
            injectionBinder.Bind<LeaderboardManager>()
                .To<LeaderboardManager>()
                .ToSingleton()
                .CrossContext();
        }

        void IModuleConfig.PostBindings(IInjectionBinder injectionBinder) {
            var eventBus = injectionBinder.GetInstance<EventBus>();
            var leaderboardsScreenController =
                injectionBinder.GetInstance<LeaderboardsScreenController>();
            eventBus.On(LeaderboardsScreenView.Event.BackBtnClick,
                leaderboardsScreenController.OnBackBtnClick);
        }
    }
}
