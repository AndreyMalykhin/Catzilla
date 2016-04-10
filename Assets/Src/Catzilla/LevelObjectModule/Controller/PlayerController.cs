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
        private PlayerStateStorage playerStateStorage;

        [Inject]
        private LevelSettingsStorage levelSettingsStorage;

        [Inject]
        private PoolStorageView poolStorage;

        [Inject]
        private AudioManager audioManager;

        [Inject]
        private Server server;

        [Inject("EffectsHighPrioAudioChannel")]
        private int effectsHighPrioAudioChannel;

        [Inject("PlayerHighPrioAudioChannel")]
        private int playerHighPrioAudioChannel;

        [Inject("PlayerLowPrioAudioChannel")]
        private int playerLowPrioAudioChannel;

        [Inject("PlayStopwatch")]
        private Stopwatch playStopwatch;

        [Inject]
        private Translator translator;

        [Inject("MainCamera")]
        private Camera mainCamera;

        [Inject]
        private PlayerManager playerManager;

        [Inject]
        private WorldSpacePopupManagerView worldSpacePopupManager;

        [Inject("SpeechWorldPopupType")]
        private int speechWorldPopupType;

        private PlayerView player;

        public void OnViewConstruct(Evt evt) {
            player = (PlayerView) evt.Source;
            playStopwatch.Reset();
            playStopwatch.Start();
            mainCamera.gameObject.SetActive(false);
        }

        public void OnViewDestroy(Evt evt) {
            if (mainCamera != null) {
                mainCamera.gameObject.SetActive(true);
            }
        }

        public void OnTriggerEnter(Evt evt) {
            var collider = (Collider) evt.Data;
            Rigidbody colliderBody = collider.attachedRigidbody;

            if (collider == null || colliderBody == null) {
                return;
            }

            Profiler.BeginSample("PlayerController.OnTriggerEnter()");
            var colliderDamaging = colliderBody.GetComponent<DamagingView>();

            if (colliderDamaging != null) {
                playerManager.Attack(player, colliderDamaging);
            }

            var colliderTreating = colliderBody.GetComponent<TreatingView>();

            if (colliderTreating != null) {
                player.Health += colliderTreating.AddHealth;
            }

            var colliderReward = colliderBody.GetComponent<RewardBonusView>();

            if (colliderReward != null) {
                playerManager.ApplyRewardBonus(player, colliderReward);
            }

            var colliderResurrection =
                colliderBody.GetComponent<ResurrectionBonusView>();

            if (colliderResurrection != null) {
                playerManager.ApplyResurrectionBonus(
                    player, colliderResurrection);
            }

            var colliderScore = colliderBody.GetComponent<ScoreBonusView>();

            if (colliderScore != null) {
                ++player.ScoreBonusesTaken;
            }

            var colliderScoreable =
                colliderBody.GetComponent<ScoreableView>();
            int addedScore = 0;

            if (colliderScoreable != null) {
                addedScore = playerManager.AddScore(player, colliderScoreable);
            }

            var colliderSmashable = colliderBody.GetComponent<SmashableView>();

            if (colliderSmashable != null) {
                if (colliderBody.GetComponent<CopView>() != null) {
                    ++player.SmashedCops;
                }

                player.AddSmash(addedScore);
            }

            Profiler.EndSample();
        }

        public void OnDeath(Evt evt) {
            playStopwatch.Stop();
            playerManager.Loose(player);
        }

        public void OnFootstep(Evt evt) {
            if (player.FootstepSound != null) {
                var pitch = UnityEngine.Random.Range(0.95f, 1.05f);
                audioManager.Play(
                    player.FootstepSound,
                    player.LowPrioAudioSource,
                    playerLowPrioAudioChannel,
                    pitch);
            }
        }

        public void OnScoreChange(Evt evt) {
            LevelSettings levelSettings =
                levelSettingsStorage.Get(playerStateStorage.Get().Level);

            if (player.Score < levelSettings.CompletionScore) {
                return;
            }

            playStopwatch.Stop();
            playerManager.CompleteLevel(player);
        }

        public void OnHealthChange(Evt evt) {
            int oldHealth = (int) evt.Data;

            if (player.Health < oldHealth) {
                if (player.Health <= 0 || player.HurtSound == null) {
                    return;
                }

                var pitch = UnityEngine.Random.Range(0.95f, 1.05f);
                audioManager.Play(
                    player.HurtSound,
                    player.HighPrioAudioSource,
                    playerHighPrioAudioChannel,
                    pitch);
            } else if (player.Health > oldHealth) {
                if (player.TreatSound == null) {
                    return;
                }

                var pitch = UnityEngine.Random.Range(0.95f, 1.05f);
                audioManager.Play(player.TreatSound, player.HighPrioAudioSource,
                    playerHighPrioAudioChannel, pitch);
            }
        }

        public void OnShockwave(Evt evt) {
            var shockwavable = (ShockwavableView) evt.Source;
            Vector3 cameraShakeAmount = shockwavable.CameraShakeDirection *
                UnityEngine.Random.Range(shockwavable.CameraShakeMinAmount,
                    shockwavable.CameraShakeMaxAmount);
            player.ShakeCamera(
                cameraShakeAmount,
                shockwavable.CameraShakeDuration,
                shockwavable.ShakeCameraInOneDirection);
        }

        public void OnSmashStreak(Evt evt) {
            PlayerState playerState = playerStateStorage.Get();
            var streak = (PlayerView.SmashStreak) evt.Data;
            playerState.GetRecord(GooglePlayIds.leaderboard_smash_streak_scores)
                .Value = streak.ExtraScore;

            if (player.SmashStreakSound != null) {
                audioManager.Play(
                    player.SmashStreakSound,
                    player.HighPrioAudioSource,
                    playerHighPrioAudioChannel);
            }
        }

        public void OnRefuse(Evt evt) {
            // DebugUtils.Log("PlayerController.OnRefuse()");
            if (player.RefuseSound != null) {
                audioManager.Play(
                    player.RefuseSound,
                    player.LowPrioAudioSource,
                    playerLowPrioAudioChannel);
            }

            var popup = (WorldSpaceTextPopupView) worldSpacePopupManager.Get(
                speechWorldPopupType);
            popup.Msg.text = translator.Translate("Player.Refuse");
            popup.LookAtTarget = player.Camera;
            popup.PlaceAbove(player.Collider.bounds);
            worldSpacePopupManager.Show(popup);
        }

        public void OnResurrect(Evt evt) {
            playStopwatch.Reset();
            playStopwatch.Start();
            CleanProjectiles();
        }

        private void CleanProjectiles() {
            var projectiles = (ProjectileView[]) GameObject.FindObjectsOfType(
                typeof(ProjectileView));

            for (int i = 0; i < projectiles.Length; ++i) {
                poolStorage.ReturnLater(
                    projectiles[i].GetComponent<PoolableView>());
            }
        }
    }
}
