using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using GameSparks.Core;
using GameSparks.Api;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.context.api;
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

        public float ConnectionTimeout {get; set;}
        public int PendingRequestsCount {get; protected set;}
        public bool IsConnected {get; protected set;}
        public bool IsDisposed {get; protected set;}

        [PostConstruct]
        public virtual void OnConstruct() {
            ConnectionTimeout = 5f;
        }

        public virtual void Connect(Action onSuccess, Action onFail = null) {
            // DebugUtils.Log("Server.Connect()");

            if (!GS.Available) {
                OnRequest();
                CoroutineManager.Run(WaitForConnection(onSuccess, onFail));
                return;
            }

            IsConnected = true;
            onSuccess();
        }

        public virtual void Login(
            string name, Action onSuccess, Action onFail = null) {
            // DebugUtils.Log("Server.Login()");
            new DeviceAuthenticationRequest()
                .SetDisplayName(name)
                .Send((response) => {
                    OnResponse();

                    if (response.HasErrors) {
                        if (onFail != null) onFail();
                        return;
                    }

                    onSuccess();
                });
            OnRequest();
        }

        public virtual void LinkFacebookAccount(
            string accessToken, Action onSuccess, Action onFail = null) {
            // DebugUtils.Log("Server.LinkFacebookAccount()");
            new FacebookConnectRequest()
                .SetAccessToken(accessToken)
                .SetErrorOnSwitch(true)
                .Send((response) => {
                    OnResponse();

                    if (response.HasErrors) {
                        if (onFail != null) onFail();
                        return;
                    }

                    onSuccess();
                });
            OnRequest();
        }

        public virtual void GetScoreLeaderboard(
            Action<List<ScoreLeaderboardItem>> onSuccess,
            Action onFail = null) {
            int itemsCount = 10;
            new AroundMeLeaderboardRequest()
                .SetEntryCount(itemsCount)
                .SetLeaderboardShortCode("ScoreLeaderboard")
                .Send((response) => {
                    OnResponse();

                    if (response.HasErrors) {
                        if (onFail != null) onFail();
                        return;
                    }

                    var items = new List<ScoreLeaderboardItem>(itemsCount);

                    foreach (AroundMeLeaderboardResponse._LeaderboardData item in response.Data) {
                        items.Add(new ScoreLeaderboardItem(
                            item.Rank.GetValueOrDefault(),
                            Int32.Parse(item.JSONData["ScoreRecord"].ToString()),
                            item.UserName
                        ));
                    }

                    onSuccess(items);
                });
            OnRequest();
        }

        public virtual void SavePlayer(PlayerState player,
            Action onSuccess = null, Action onFail = null) {
            // DebugUtils.Log("Server.SavePlayer()");
            new LogEventRequest()
                .SetEventKey("PlayerSave")
                .SetEventAttribute("Level", player.Level)
                .SetEventAttribute("ScoreRecord", player.ScoreRecord)
                .Send((response) => {
                    OnResponse();

                    if (response.HasErrors) {
                        if (onFail != null) onFail();
                        return;
                    }

                    if (onSuccess != null) onSuccess();
                });
            OnRequest();
        }

        public virtual void GetPlayer(
            Action<PlayerState> onSuccess, Action onFail = null) {
            // DebugUtils.Log("Server.GetPlayer()");
            new LogEventRequest().SetEventKey("PlayerGet").Send((response) => {
                OnResponse();

                if (response.HasErrors) {
                    if (onFail != null) onFail();
                    return;
                }

                GSData playerData = response.ScriptData.GetGSData("Player");
                PlayerState player = null;

                if (playerData != null) {
                    player = new PlayerState{
                        Level = playerData.GetInt("Level").GetValueOrDefault(),
                        ScoreRecord = playerData.GetInt("ScoreRecord")
                            .GetValueOrDefault()
                    };
                }

                onSuccess(player);
            });
            OnRequest();
        }

        public virtual void Dispose() {
            // DebugUtils.Log("Server.Dispose()");
            GS.ShutDown();
            IsConnected = false;
            IsDisposed = true;
            EventBus.Fire(Event.Dispose, new Evt(this));
        }

        private IEnumerator WaitForConnection(
            Action onSuccess, Action onFail = null) {
            float timeLeft = ConnectionTimeout;
            float checkPeriod = 0.1f;

            while (!GS.Available) {
                if (timeLeft <= 0f) {
                    OnResponse();
                    if (onFail != null) onFail();
                    yield break;
                }

                yield return new WaitForSeconds(checkPeriod);
                timeLeft -= checkPeriod;
            }

            IsConnected = true;
            OnResponse();
            onSuccess();
        }

        private void OnRequest() {
            ++PendingRequestsCount;
            EventBus.Fire(Event.Request, new Evt(this));
        }

        private void OnResponse() {
            --PendingRequestsCount;
            EventBus.Fire(Event.Response, new Evt(this));
        }
    }
}
