using UnityEngine;
using System.Collections;
using strange.extensions.injector.api;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.Config;
using Catzilla.CommonModule.View;
using Catzilla.CommonModule.Model;
using Catzilla.LevelObjectModule.View;
using Catzilla.PlayerModule.Model;
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
            var rewardEarnDlg = GameObject.FindWithTag(
                "GameOverScreen.RewardEarnDlg").GetComponent<DlgView>();
            injectionBinder.Bind<DlgView>()
                .ToValue(rewardEarnDlg)
                .ToName("RewardEarnDlg")
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
            eventBus.AddListener(GameOverMenuView.Event.RewardBtnClick,
                gameOverMenuController.OnRewardBtnClick);
            eventBus.AddListener(GameOverMenuView.Event.LeaderboardBtnClick,
                gameOverMenuController.OnLeaderboardBtnClick);
            eventBus.AddListener(Server.Event.Dispose,
                gameOverMenuController.OnServerDispose);
            eventBus.AddListener(PlayerView.Event.Construct,
                gameOverMenuController.OnPlayerConstruct);
            eventBus.AddListener(PlayerStateStorage.Event.Save,
                gameOverMenuController.OnPlayerStateStorageSave);
        }
    }
}
