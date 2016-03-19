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
            container.Bind<GameOverScreenController>().ToSingle();
        }

        public override void PostBindings(DiContainer container) {
            var eventBus = container.Resolve<EventBus>();
            var gameOverScreenController =
                container.Resolve<GameOverScreenController>();
            eventBus.On(PlayerView.Event.Construct,
                gameOverScreenController.OnPlayerConstruct);
            eventBus.On(PlayerStateStorage.Event.Save,
                gameOverScreenController.OnPlayerStateStorageSave);
            var gameOverScreen = container.Resolve<GameOverScreenView>();
            gameOverScreen.Menu.ExitBtn.onClick.AddListener(
                gameOverScreenController.OnExitBtnClick);
            gameOverScreen.Menu.RestartBtn.onClick.AddListener(
                gameOverScreenController.OnRestartBtnClick);
            gameOverScreen.Menu.ResurrectBtn.onClick.AddListener(
                gameOverScreenController.OnResurrectBtnClick);
            gameOverScreen.Menu.RewardBtn.onClick.AddListener(
                gameOverScreenController.OnRewardBtnClick);
        }
    }
}
