using UnityEngine;
using System.Diagnostics;
using System.Text;
using Zenject;
using Catzilla.CommonModule.View;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.Model;
using Catzilla.LevelObjectModule.View;
using Catzilla.LevelModule.View;

namespace Catzilla.PlayerModule.Model {
    public class PlayerManager {
        [Inject]
        private WorldSpacePopupManager popupManager;

        [Inject]
        private EventBus eventBus;

        [Inject]
        private PlayerStateStorage playerStateStorage;

        [Inject("PlayStopwatch")]
        private Stopwatch playStopwatch;

        [Inject]
        private Server server;

        [Inject]
        private LevelCompleteScreenView levelCompleteScreen;

        [Inject]
        private Translator translator;

        [Inject]
        private Game game;

        [Inject("ScoreWorldPopupType")]
        private int scoreWorldPopupType;

        [Inject("ResurrectionWorldPopupType")]
        private int resurrectionWorldPopupType;

        [Inject("RewardWorldPopupType")]
        private int rewardWorldPopupType;

        private readonly StringBuilder strBuilder = new StringBuilder(8);

        public void AddScore(PlayerView player, ScoreableView scoreable) {
            if (player.IsScoreFreezed) {
                return;
            }

            int score = UnityEngine.Random.Range(
                scoreable.MinScore, scoreable.MaxScore + 1);
            player.Score += score;
            WorldSpacePopupView popup = popupManager.Get(scoreWorldPopupType);
            popup.Msg.text =
                strBuilder.Append('+').Append(score).ToString();
            strBuilder.Length = 0;
            popup.LookAtTarget = player.Camera;
            popup.PlaceAbove(scoreable.Collider.bounds);
            popupManager.Show(popup);
        }

        public void ApplyResurrectionBonus(
            PlayerView player, ResurrectionBonusView resurrectionBonus) {
            ++player.ResurrectionBonusesTaken;
            int addResurrectionsCount = 1;
            playerStateStorage.Get().AvailableResurrectionsCount +=
                addResurrectionsCount;
            WorldSpacePopupView popup =
                popupManager.Get(resurrectionWorldPopupType);
            popup.Msg.text = translator.Translate(
                "ResurrectionBonus.Take", addResurrectionsCount);
            popup.LookAtTarget = player.Camera;
            popup.PlaceAbove(resurrectionBonus.Collider.bounds);
            popupManager.Show(popup);
        }

        public void ApplyRewardBonus(
            PlayerView player, RewardBonusView rewardBonus) {
            int addRewardsCount = 1;
            playerStateStorage.Get().AvailableRewardsCount += addRewardsCount;
            WorldSpacePopupView popup = popupManager.Get(rewardWorldPopupType);
            popup.Msg.text =
                translator.Translate("RewardBonus.Take", addRewardsCount);
            popup.LookAtTarget = player.Camera;
            popup.PlaceAbove(rewardBonus.Collider.bounds);
            popupManager.Show(popup);
        }

        public void CompleteLevel(PlayerView player) {
            // DebugUtils.Log("PlayerManager.CompleteLevel()");
            eventBus.Fire((int) Events.PlayerManagerPreLevelComplete,
                new Evt(this, player));
            PlayerState playerState = playerStateStorage.Get();
            GiveAchievementIfNeeded(playerState);
            player.IsHealthFreezed = true;
            player.IsScoreFreezed = true;
            ++playerState.Level;
            playerState.ScoreRecord = player.Score;
            playerState.PlayTime += playStopwatch.Elapsed;
            playerStateStorage.Save(playerState);

            if (server.IsLoggedIn) {
                playerStateStorage.Sync(server);
            }

            levelCompleteScreen.Score.text =
                translator.Translate("LevelCompleteScreen.Score", player.Score);
            var showable = levelCompleteScreen.GetComponent<ShowableView>();
            showable.OnShow += OnLevelCompleteScreenShow;
            showable.Show();
        }

        private void OnLevelCompleteScreenShow(ShowableView showable) {
            showable.OnShow -= OnLevelCompleteScreenShow;
            game.UnloadLevel();
        }

        private void GiveAchievementIfNeeded(PlayerState playerState) {
            string achievementId;

            switch (playerState.Level + 1) {
                case 5:
                    achievementId = GooglePlayIds.achievement_complete_level_5;
                    break;
                case 10:
                    achievementId = GooglePlayIds.achievement_complete_level_10;
                    break;
                case 15:
                    achievementId = GooglePlayIds.achievement_complete_level_15;
                    break;
                case 20:
                    achievementId = GooglePlayIds.achievement_complete_level_20;
                    break;
                case 25:
                    achievementId = GooglePlayIds.achievement_complete_level_25;
                    break;
                default:
                    return;
            }

            playerState.AddAchievement(new Achievement(achievementId));
        }
    }
}
