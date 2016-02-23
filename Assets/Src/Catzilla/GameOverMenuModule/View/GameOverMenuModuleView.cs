using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;
using Catzilla.CommonModule.Model;
using Catzilla.LevelObjectModule.View;
using Catzilla.PlayerModule.Model;
using Catzilla.GameOverMenuModule.Controller;
using Catzilla.GameOverMenuModule.View;

namespace Catzilla.GameOverMenuModule.View {
    public class GameOverMenuModuleView: ModuleView {
        [SerializeField]
        private GameOverScreenView screen;

        [SerializeField]
        private GameOverMenuView menu;

        public override void InitBindings(DiContainer container) {
            container.Bind<GameOverScreenView>().ToInstance(screen);
            container.Bind<GameOverMenuView>().ToInstance(menu);
            container.Bind<GameOverMenuController>().ToSingle();
        }

        public override void PostBindings(DiContainer container) {
            var eventBus = container.Resolve<EventBus>();
            var gameOverMenuController =
                container.Resolve<GameOverMenuController>();
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
