using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SmartLocalization;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.Model;
using Catzilla.CommonModule.Util;
using Catzilla.LevelObjectModule.View;
using Catzilla.PlayerModule.Model;
using Catzilla.GameOverMenuModule.View;

namespace Catzilla.GameOverMenuModule.Controller {
    public class GameOverMenuController {
        [Inject]
        public GameOverMenuView GameOverMenu {get; set;}

        [Inject]
        public GameOverScreenView GameOverScreen {get; set;}

        [Inject]
        public PlayerStateStorage PlayerStateStorage {get; set;}

        [Inject]
        public Game Game {get; set;}

        [Inject]
        public Ad Ad {get; set;}

        private PlayerView player;

        public void OnExitBtnClick() {
            Game.Exit();
        }

        public void OnRestartBtnClick() {
            Game.LoadLevel();
            Game.Resume();
            GameOverScreen.Hide();
        }

        public void OnPlayerConstruct(IEvent evt) {
            player = (PlayerView) evt.data;
        }

        public void OnResurrectBtnClick() {
            Ad.Show(OnAdShowFinish);
        }

        private void OnAdShowFinish() {
            player.IsHealthFreezed = false;
            player.IsScoreFreezed = false;
            player.Resurrect();
            PlayerState playerState = PlayerStateStorage.Get();
            --playerState.AvailableResurrectionsCount;
            Game.Resume();
            GameOverScreen.Hide();
        }
    }
}
