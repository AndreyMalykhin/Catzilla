using UnityEngine;
using System.Collections;
using strange.extensions.injector.api;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.Config;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;
using Catzilla.CommonModule.Model;
using Catzilla.LevelObjectModule.View;
using Catzilla.PlayerModule.Model;
using Catzilla.GameOverMenuModule.Controller;
using Catzilla.GameOverMenuModule.View;

namespace Catzilla.GameOverMenuModule.Config {
    [CreateAssetMenuAttribute]
    public class GameOverMenuModuleConfig: ModuleConfig {
        public override void InitBindings(IInjectionBinder injectionBinder) {
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

        public override void PostBindings(IInjectionBinder injectionBinder) {
            var eventBus = injectionBinder.GetInstance<EventBus>();
            var gameOverMenuController =
                injectionBinder.GetInstance<GameOverMenuController>();
            eventBus.On(GameOverMenuView.Event.ExitBtnClick,
                gameOverMenuController.OnExitBtnClick);
            eventBus.On(GameOverMenuView.Event.RestartBtnClick,
                gameOverMenuController.OnRestartBtnClick);
            eventBus.On(GameOverMenuView.Event.ResurrectBtnClick,
                gameOverMenuController.OnResurrectBtnClick);
            eventBus.On(GameOverMenuView.Event.RewardBtnClick,
                gameOverMenuController.OnRewardBtnClick);
            eventBus.On(GameOverMenuView.Event.LeaderboardBtnClick,
                gameOverMenuController.OnLeaderboardBtnClick);
            eventBus.On(Server.Event.Dispose,
                gameOverMenuController.OnServerDispose);
            eventBus.On(PlayerView.Event.Construct,
                gameOverMenuController.OnPlayerConstruct);
            eventBus.On(PlayerStateStorage.Event.Save,
                gameOverMenuController.OnPlayerStateStorageSave);
        }
    }
}
