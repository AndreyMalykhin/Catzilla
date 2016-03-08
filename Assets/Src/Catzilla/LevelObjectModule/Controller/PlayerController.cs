using UnityEngine;
using System;
using System.Diagnostics;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;
using Catzilla.CommonModule.Model;
using Catzilla.PlayerModule.Model;
using Catzilla.GameOverMenuModule.View;
using Catzilla.LevelModule.View;
using Catzilla.LevelModule.Model;
using Catzilla.LevelObjectModule.View;

namespace Catzilla.LevelObjectModule.Controller {
    public class PlayerController {
        [Inject]
        public GameOverScreenView GameOverScreen {get; set;}

        [Inject]
        public PlayerStateStorage PlayerStateStorage {get; set;}

        [Inject]
        public LevelCompleteScreenView LevelCompleteScreen {get; set;}

        [Inject]
        public LevelSettingsStorage LevelSettingsStorage {get; set;}

        [Inject]
        public PoolStorageView PoolStorage {get; set;}

        [Inject]
        public AudioManager AudioManager {get; set;}

        [Inject]
        public Server Server {get; set;}

        [Inject]
        public Game Game {get; set;}

        [Inject("ProjectileTag")]
        public string ProjectileTag {get; set;}

        [Inject("PlayerHighPrioAudioChannel")]
        public int PlayerHighPrioAudioChannel {get; set;}

        [Inject("PlayerLowPrioAudioChannel")]
        public int PlayerLowPrioAudioChannel {get; set;}

        [Inject("PlayStopwatch")]
        public Stopwatch PlayStopwatch {get; set;}

        [Inject]
        public GiftManager GiftManager {get; set;}

        [Inject]
        public ScreenSpacePopupManagerView PopupManager {get; set;}

        [Inject]
        public Translator Translator {get; set;}

        [Inject("CommonPopupType")]
        public int CommonPopupType {get; set;}

        [Inject]
        public RewardManager RewardManager {get; set;}

        private PlayerView player;

        public void OnViewConstruct(Evt evt) {
            player = (PlayerView) evt.Source;
        }

        public void OnDeath(Evt evt) {
            PlayerState playerState = PlayerStateStorage.Get();
            AnalyticsUtils.AddCategorizedEventParam("Level", playerState.Level);
            AnalyticsUtils.LogEvent("Player.Death");
            playerState.ScoreRecord = player.Score;
            PlayStopwatch.Stop();
            playerState.PlayTime += PlayStopwatch.Elapsed;
            PlayerStateStorage.Save(playerState);

            if (Server.IsLoggedIn) {
                PlayerStateStorage.Sync(Server);
            }

            var showable = GameOverScreen.GetComponent<ShowableView>();
            showable.OnShow = OnGameOverScreenShow;
            showable.Show();

            if (player.DeathSound != null) {
                AudioManager.Play(player.DeathSound, player.HighPrioAudioSource,
                    PlayerHighPrioAudioChannel);
            }
        }

        public void OnFootstep(Evt evt) {
            if (player.FootstepSound == null) {
                return;
            }

            var pitch = UnityEngine.Random.Range(0.9f, 1.1f);
            AudioManager.Play(
                player.FootstepSound,
                player.LowPrioAudioSource,
                PlayerLowPrioAudioChannel,
                pitch);
        }

        public void OnScoreChange(Evt evt) {
            LevelSettings levelSettings =
                LevelSettingsStorage.Get(PlayerStateStorage.Get().Level);

            if (player.Score < levelSettings.CompletionScore) {
                return;
            }

            CompleteLevel(player);
        }

        public void OnHealthChange(Evt evt) {
            int oldHealth = (int) evt.Data;

            if (player.Health < oldHealth) {
                if (player.Health <= 0 || player.HurtSound == null) {
                    return;
                }

                var pitch = UnityEngine.Random.Range(0.9f, 1.1f);
                AudioManager.Play(
                    player.HurtSound,
                    player.HighPrioAudioSource,
                    PlayerHighPrioAudioChannel,
                    pitch);
            } else if (player.Health > oldHealth) {
                if (player.TreatSound == null) {
                    return;
                }

                var pitch = UnityEngine.Random.Range(0.9f, 1.1f);
                AudioManager.Play(player.TreatSound, player.HighPrioAudioSource,
                    PlayerHighPrioAudioChannel, pitch);
            }
        }

        public void OnResurrect(Evt evt) {
            CleanProjectiles();
            AnalyticsUtils.AddCategorizedEventParam(
                "Level", PlayerStateStorage.Get().Level);
            AnalyticsUtils.LogEvent("Player.Resurrection");
        }

        private void OnGameOverScreenShow(ShowableView showable) {
            Game.Pause();
        }

        private void CleanProjectiles() {
            GameObject[] projectiles =
                GameObject.FindGameObjectsWithTag(ProjectileTag);

            for (int i = 0; i < projectiles.Length; ++i) {
                PoolStorage.Return(projectiles[i].GetComponent<PoolableView>());
            }
        }

        private void CompleteLevel(PlayerView player) {
            // DebugUtils.Log(
            //     "PlayerController.CompleteLevel(); {0}", DateTime.Now);
            player.IsHealthFreezed = true;
            player.IsScoreFreezed = true;
            PlayerState playerState = PlayerStateStorage.Get();
            AnalyticsUtils.AddCategorizedEventParam("Level", playerState.Level);
            AnalyticsUtils.LogEvent("Level.Completion");
            GiveAchievementIfNeeded(playerState);
            int givenResurrectionsCount = GiftManager.Give(playerState);
            ScreenSpacePopupView popup = PopupManager.Get(CommonPopupType);
            popup.Msg.text = Translator.Translate(
                "Player.GiftEarn", givenResurrectionsCount);
            PopupManager.Show(popup);
            int unlockedRewardsCount = RewardManager.Unlock(playerState);
            popup = PopupManager.Get(CommonPopupType);
            popup.Msg.text = Translator.Translate(
                "Player.RewardUnlock", unlockedRewardsCount);
            PopupManager.Show(popup);
            ++playerState.Level;
            playerState.ScoreRecord = player.Score;
            PlayStopwatch.Stop();
            playerState.PlayTime += PlayStopwatch.Elapsed;
            PlayerStateStorage.Save(playerState);

            if (Server.IsLoggedIn) {
                PlayerStateStorage.Sync(Server);
            }

            LevelCompleteScreen.OnHide = OnLevelCompleteScreenHide;
            LevelCompleteScreen.Show();
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

        private void OnLevelCompleteScreenHide() {
            Game.LoadLevel();
        }
    }
}
