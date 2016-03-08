using UnityEngine;
using UnityEngine.SocialPlatforms;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Zenject;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;
using Catzilla.PlayerModule.Model;

namespace Catzilla.CommonModule.Model {
    [CreateAssetMenuAttribute]
    public class Server: ScriptableObject, IDisposable {
        public enum Event {Request, Response, Dispose}

        [Inject]
        public CoroutineManagerView CoroutineManager {get; set;}

        [Inject]
        public EventBus EventBus {get; set;}

        public int PendingRequestsCount {get {return pendingRequestsCount;}}
        public bool IsConnected {get {return isConnected;}}
        public bool IsDisposed {get {return isDisposed;}}
        public bool IsLoggedIn {get {return isLoggedIn;}}

        [NonSerialized]
        protected const string savedGameFilename = "last";

        [NonSerialized]
        protected int pendingRequestsCount;

        [NonSerialized]
        protected bool isConnected;

        [NonSerialized]
        protected bool isDisposed;

        [NonSerialized]
        protected bool isLoggedIn;

        public virtual void Connect(Action onSuccess, Action onFail = null) {
            // DebugUtils.Log("Server.Connect()");
            PlayGamesClientConfiguration config =
                new PlayGamesClientConfiguration.Builder()
                .EnableSavedGames()
                .Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.DebugLogEnabled = Debug.isDebugBuild;
            PlayGamesPlatform.Activate();
            isConnected = true;
            isLoggedIn = Social.localUser.authenticated;
            onSuccess();
        }

        public virtual void Login(Action onSuccess, Action onFail = null) {
            // DebugUtils.Log("Server.Login()");
            if (isLoggedIn) {
                onSuccess();
                return;
            }

            OnRequest();
            Social.localUser.Authenticate((bool isSuccess) => {
                OnResponse(isSuccess);

                if (!isSuccess) {
                    if (onFail != null) onFail();
                    return;
                }

                isLoggedIn = true;
                onSuccess();
            });
        }

        public virtual void SavePlayer(PlayerState player,
            Action onSuccess = null, Action onFail = null) {
            DebugUtils.Assert(isLoggedIn);
            OnRequest();
            OpenSavedGame((SavedGameRequestStatus requestStatus, ISavedGameMetadata game) => {
                if (requestStatus != SavedGameRequestStatus.Success) {
                    OnResponse(false);
                    if (onFail != null) onFail();
                    return;
                }

                string jsonPlayer = JsonUtility.ToJson(player);
                DebugUtils.Log("Server.SavePlayer(); player={0}", jsonPlayer);
                byte[] binaryPlayer = Encoding.UTF8.GetBytes(jsonPlayer);
                SavedGameMetadataUpdate metadataUpdate =
                    new SavedGameMetadataUpdate.Builder()
                        .WithUpdatedPlayedTime(player.PlayTime)
                        .Build();
                PlayGamesPlatform.Instance.SavedGame.CommitUpdate(
                    game,
                    metadataUpdate,
                    binaryPlayer,
                    (SavedGameRequestStatus requestStatus2, ISavedGameMetadata game2) => {
                        if (requestStatus2 != SavedGameRequestStatus.Success) {
                            OnResponse(false);
                            if (onFail != null) onFail();
                            return;
                        }

                        if (player.IsScoreSynced) {
                            SavePlayerAchievements(player, onSuccess, onFail);
                            return;
                        }

                        Social.ReportScore(
                            player.ScoreRecord,
                            GooglePlayIds.leaderboard_scores,
                            (bool isSuccess) => {
                                if (!isSuccess) {
                                    OnResponse(isSuccess);
                                    if (onFail != null) onFail();
                                    return;
                                }

                                player.IsScoreSynced = true;
                                SavePlayerAchievements(
                                    player, onSuccess, onFail);
                            });
                    });
            });
        }

        public virtual void GetPlayer(
            Action<PlayerState> onSuccess, Action onFail = null) {
            DebugUtils.Assert(isLoggedIn);
            OnRequest();
            OpenSavedGame((SavedGameRequestStatus requestStatus, ISavedGameMetadata game) => {
                if (requestStatus != SavedGameRequestStatus.Success) {
                    OnResponse(false);
                    if (onFail != null) onFail();
                    return;
                }

                PlayGamesPlatform.Instance.SavedGame.ReadBinaryData(
                    game,
                    (SavedGameRequestStatus requestStatus2, byte[] binaryPlayer) => {
                        bool isSuccess =
                            requestStatus2 == SavedGameRequestStatus.Success;
                        OnResponse(isSuccess);

                        if (!isSuccess) {
                            if (onFail != null) onFail();
                            return;
                        }

                        PlayerState player = null;

                        if (binaryPlayer.Length > 0) {
                            string jsonPlayer =
                                Encoding.UTF8.GetString(binaryPlayer);
                            // DebugUtils.Log(
                            //     "Server.GetPlayer(); player={0}", jsonPlayer);
                            player =
                                JsonUtility.FromJson<PlayerState>(jsonPlayer);
                        }

                        onSuccess(player);
                    });
            });
        }

        public virtual void Dispose() {
            // DebugUtils.Log("Server.Dispose()");
            DebugUtils.Assert(!isDisposed);
            isConnected = false;
            isLoggedIn = false;
            isDisposed = true;
            EventBus.Fire(Event.Dispose, new Evt(this));
        }

        private void OnRequest() {
            ++pendingRequestsCount;
            // DebugUtils.Log("Server.OnRequest(); pendingRequestsCount={0}",
            //     PendingRequestsCount);
            EventBus.Fire(Event.Request, new Evt(this));
        }

        private void OnResponse(bool isSuccess) {
            --pendingRequestsCount;
            // DebugUtils.Log("Server.OnResponse(); pendingRequestsCount={0}",
            //     PendingRequestsCount);
            EventBus.Fire(Event.Response, new Evt(this, isSuccess));
        }

        private void OpenSavedGame(
            Action<SavedGameRequestStatus, ISavedGameMetadata> onDone) {
            PlayGamesPlatform.Instance.SavedGame
                .OpenWithAutomaticConflictResolution(
                    savedGameFilename,
                    DataSource.ReadCacheOrNetwork,
                    ConflictResolutionStrategy.UseLongestPlaytime,
                    onDone);
        }

        private void SavePlayerAchievements(
            PlayerState player, Action onSuccess = null, Action onFail = null) {
            List<Catzilla.PlayerModule.Model.Achievement> achievements =
                player.Achievements;
            bool isSuccess = true;

            if (achievements.Count == 0) {
                OnResponse(isSuccess);
                if (onSuccess != null) onSuccess();
                return;
            }

            int savedAchievementsCount = 0;

            for (int i = 0; i < achievements.Count; ++i) {
                var achievement = achievements[i];

                if (achievement.IsSynced) {
                    ++savedAchievementsCount;
                    continue;
                }

                Social.ReportProgress(
                    achievement.Id,
                    100f,
                    (bool isSuccess2) => {
                        if (!isSuccess2) {
                            OnResponse(isSuccess2);
                            if (onFail != null) onFail();
                            return;
                        }

                        achievement.IsSynced = true;
                        ++savedAchievementsCount;

                        if (savedAchievementsCount < achievements.Count) {
                            return;
                        }

                        OnResponse(isSuccess2);
                        if (onSuccess != null) onSuccess();
                    });
            }

            if (savedAchievementsCount < achievements.Count) {
                return;
            }

            OnResponse(isSuccess);
            if (onSuccess != null) onSuccess();
        }
    }
}
