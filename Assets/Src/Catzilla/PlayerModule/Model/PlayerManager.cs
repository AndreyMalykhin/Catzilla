using UnityEngine;
using System;
using System.Diagnostics;
using System.Text;
using Zenject;
using Catzilla.CommonModule.View;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.Model;
using Catzilla.GameOverMenuModule.View;
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

        [Inject("PlayerHighPrioAudioChannel")]
        private int playerHighPrioAudioChannel;

        [Inject]
        private AudioManager audioManager;

        [Inject]
        private GameOverScreenView gameOverScreen;

        private readonly StringBuilder strBuilder = new StringBuilder(8);

        /**
         * @return Added score
         */
        public int AddScore(PlayerView player, ScoreableView scoreable) {
            if (player.IsScoreFreezed) {
                return 0;
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
            return score;
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
            playerState.PlayTime += playStopwatch.Elapsed;
            SaveRecords(player, playerState);

            levelCompleteScreen.Score.text =
                translator.Translate("LevelCompleteScreen.Score", player.Score);
            var levelDuration = new TimeSpan(0, 0, (int) player.TotalLifetime);
            var formattedLevelDuration = string.Format(
                "{0:00}:{1:00}", levelDuration.Minutes, levelDuration.Seconds);
            levelCompleteScreen.Time.text = translator.Translate(
                "LevelCompleteScreen.Time", formattedLevelDuration);
            var levelCompleteScreenShowable =
                levelCompleteScreen.GetComponent<ShowableView>();
            Action<ShowableView> levelCompleteScreenShowHandler;
            levelCompleteScreenShowHandler = delegate (ShowableView showable) {
                showable.OnShow -= levelCompleteScreenShowHandler;
                game.UnloadLevel();
                playerStateStorage.Save(playerState);

                if (server.IsLoggedIn) {
                    playerStateStorage.Sync(server);
                }
            };
            levelCompleteScreenShowable.OnShow +=
                levelCompleteScreenShowHandler;
            levelCompleteScreenShowable.Show();
        }

        public void Loose(PlayerView player) {
            PlayerState playerState = playerStateStorage.Get();
            playerState.PlayTime += playStopwatch.Elapsed;
            SaveRecords(player, playerState);

            if (player.DeathSound != null) {
                audioManager.Play(player.DeathSound, player.HighPrioAudioSource,
                    playerHighPrioAudioChannel);
            }

            var gameOverScreenShowable =
                gameOverScreen.GetComponent<ShowableView>();
            Action<ShowableView> gameOverScreenShowHandler;
            gameOverScreenShowHandler = delegate (ShowableView showable) {
                showable.OnShow -= gameOverScreenShowHandler;
                playerStateStorage.Save(playerState);

                if (server.IsLoggedIn) {
                    playerStateStorage.Sync(server);
                }
            };
            gameOverScreenShowable.OnShow += gameOverScreenShowHandler;
            gameOverScreenShowable.Show();
        }

        private void SaveRecords(PlayerView player, PlayerState playerState) {
            playerState.GetRecord(GooglePlayIds.leaderboard_scores).Value =
                player.Score;
            playerState.GetRecord(GooglePlayIds.leaderboard_smashed_cops)
                .Value += player.SmashedCops;
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
