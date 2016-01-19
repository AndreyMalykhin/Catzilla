using UnityEngine;
using System.Collections;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.Model;
using Catzilla.PlayerModule.Model;
using Catzilla.GameOverMenuModule.View;
using Catzilla.LevelModule.View;
using Catzilla.LevelObjectModule.View;

namespace Catzilla.LevelObjectModule.Controller {
    public class PlayerController {
        [Inject]
        public GameOverScreenView GameOverScreen {get; set;}

        [Inject]
        public PlayerSettingsStorage PlayerSettingsStorage {get; set;}

        [Inject]
        public LevelCompleteScreenView LevelCompleteScreen {get; set;}

        [Inject]
        public Game Game {get; set;}

        private LevelView level;

        public void OnDeath() {
            GameOverScreen.Show(() => {
                Game.Pause();
            });
        }

        public void OnScoreChange(IEvent evt) {
            var player = (PlayerView) evt.data;

            if (player.Score < level.CompletionScore) {
                return;
            }

            CompleteLevel(player);
        }

        public void OnLevelReady(IEvent evt) {
            level = (LevelView) evt.data;
        }

        private void CompleteLevel(PlayerView player) {
            player.IsHealthFreezed = true;
            player.IsScoreFreezed = true;
            PlayerSettingsStorage.GetCurrent((playerSettings) => {
                ++playerSettings.Level;
                playerSettings.MaxScore = player.Score;
                PlayerSettingsStorage.Update(playerSettings, () => {
                    LevelCompleteScreen.Show();
                });
            });
        }
    }
}
