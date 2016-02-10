using UnityEngine;
using UnityEngine.SocialPlatforms;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.context.api;
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

        public int PendingRequestsCount {get; protected set;}
        public bool IsConnected {get; protected set;}
        public bool IsDisposed {get; protected set;}
        public bool IsLoggedIn {get; protected set;}

        private const string savedGameFilename = "last";

        public virtual void Connect(Action onSuccess, Action onFail = null) {
            // DebugUtils.Log("Server.Connect()");
            PlayGamesClientConfiguration config =
                new PlayGamesClientConfiguration.Builder()
                .EnableSavedGames()
                .Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.DebugLogEnabled = Debug.isDebugBuild;
            PlayGamesPlatform.Activate();
            IsConnected = true;
            IsLoggedIn = Social.localUser.authenticated;
            onSuccess();
        }

        public virtual void Login(Action onSuccess, Action onFail = null) {
            // DebugUtils.Log("Server.Login()");
            if (IsLoggedIn) {
                onSuccess();
                return;
            }

            OnRequest();
            Social.localUser.Authenticate((bool isSuccess) => {
                OnResponse();

                if (!isSuccess) {
                    if (onFail != null) onFail();
                    return;
                }

                IsLoggedIn = true;
                onSuccess();
            });
        }

        public virtual void SavePlayer(PlayerState player,
            Action onSuccess = null, Action onFail = null) {
            DebugUtils.Assert(IsLoggedIn);
            OnRequest();
            OpenSavedGame((SavedGameRequestStatus requestStatus, ISavedGameMetadata game) => {
                if (requestStatus != SavedGameRequestStatus.Success) {
                    OnResponse();
                    if (onFail != null) onFail();
                    return;
                }

                string jsonPlayer = JsonUtility.ToJson(player);
                DebugUtils.Log("Server.SavePlayer(); player={0}", jsonPlayer);
                byte[] binaryPlayer = Encoding.Unicode.GetBytes(jsonPlayer);
                SavedGameMetadataUpdate metadataUpdate =
                    new SavedGameMetadataUpdate.Builder().Build();
                PlayGamesPlatform.Instance.SavedGame.CommitUpdate(
                    game,
                    metadataUpdate,
                    binaryPlayer,
                    (SavedGameRequestStatus requestStatus2, ISavedGameMetadata game2) => {
                        if (requestStatus2 != SavedGameRequestStatus.Success) {
                            OnResponse();
                            if (onFail != null) onFail();
                            return;
                        }

                        Social.ReportScore(
                            player.ScoreRecord,
                            GooglePlayIds.leaderboard_scores,
                            (bool isSuccess) => {
                                OnResponse();

                                if (!isSuccess) {
                                    if (onFail != null) onFail();
                                    return;
                                }

                                if (onSuccess != null) onSuccess();
                            });
                    });
            });
        }

        public virtual void GetPlayer(
            Action<PlayerState> onSuccess, Action onFail = null) {
            DebugUtils.Assert(IsLoggedIn);
            OnRequest();
            OpenSavedGame((SavedGameRequestStatus requestStatus, ISavedGameMetadata game) => {
                if (requestStatus != SavedGameRequestStatus.Success) {
                    OnResponse();
                    if (onFail != null) onFail();
                    return;
                }

                PlayGamesPlatform.Instance.SavedGame.ReadBinaryData(
                    game,
                    (SavedGameRequestStatus requestStatus2, byte[] binaryPlayer) => {
                        OnResponse();

                        if (requestStatus2 != SavedGameRequestStatus.Success) {
                            if (onFail != null) onFail();
                            return;
                        }

                        PlayerState player = null;

                        if (binaryPlayer.Length > 0) {
                            string jsonPlayer =
                                Encoding.Unicode.GetString(binaryPlayer);
                            DebugUtils.Log(
                                "Server.GetPlayer(); player={0}", jsonPlayer);
                            player =
                                JsonUtility.FromJson<PlayerState>(jsonPlayer);
                        }

                        onSuccess(player);
                    });
            });
        }

        public virtual void Dispose() {
            // DebugUtils.Log("Server.Dispose()");
            DebugUtils.Assert(!IsDisposed);
            IsConnected = false;
            IsLoggedIn = false;
            IsDisposed = true;
            EventBus.Fire(Event.Dispose, new Evt(this));
        }

        private void OnRequest() {
            ++PendingRequestsCount;
            // DebugUtils.Log("Server.OnRequest(); pendingRequestsCount={0}",
            //     PendingRequestsCount);
            EventBus.Fire(Event.Request, new Evt(this));
        }

        private void OnResponse() {
            --PendingRequestsCount;
            // DebugUtils.Log("Server.OnResponse(); pendingRequestsCount={0}",
            //     PendingRequestsCount);
            EventBus.Fire(Event.Response, new Evt(this));
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

    }
}
