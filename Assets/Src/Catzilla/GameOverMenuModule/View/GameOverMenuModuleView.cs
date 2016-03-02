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

        public override void InitBindings(DiContainer container) {
            container.Bind<GameOverScreenView>().ToInstance(screen);
            container.Bind<GameOverMenuController>().ToSingle();
        }

        public override void PostBindings(DiContainer container) {
            var eventBus = container.Resolve<EventBus>();
            var gameOverMenuController =
                container.Resolve<GameOverMenuController>();
            eventBus.On(PlayerView.Event.Construct,
                gameOverMenuController.OnPlayerConstruct);
            eventBus.On(PlayerStateStorage.Event.Save,
                gameOverMenuController.OnPlayerStateStorageSave);
            var screen = container.Resolve<GameOverScreenView>();
            screen.Menu.ExitBtn.onClick.AddListener(
                gameOverMenuController.OnExitBtnClick);
            screen.Menu.RestartBtn.onClick.AddListener(
                gameOverMenuController.OnRestartBtnClick);
            screen.Menu.ResurrectBtn.onClick.AddListener(
                gameOverMenuController.OnResurrectBtnClick);
            screen.Menu.RewardBtn.onClick.AddListener(
                gameOverMenuController.OnRewardBtnClick);
        }
    }
}
