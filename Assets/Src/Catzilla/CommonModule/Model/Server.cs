using UnityEngine;
using System;
using System.Collections;
using GameSparks.Core;
using GameSparks.Api;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.context.api;
using Catzilla.CommonModule.View;
using Catzilla.PlayerModule.Model;

namespace Catzilla.CommonModule.Model {
    public class Server: IDisposable {
        public enum Event {Request, Response}

        [Inject]
        public CoroutineManagerView CoroutineManager {get; set;}

        [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
        public IEventDispatcher EventBus {get; set;}

        public float ConnectionTimeout {get; set;}
        public int PendingRequestsCount {get; private set;}
        public bool IsConnected {get; private set;}

        [PostConstruct]
        public void OnConstruct() {
            ConnectionTimeout = 5f;
        }

        public void Connect(Action onSuccess, Action onFail = null) {
            Debug.Log("Server.Connect()");

            if (!GS.Available) {
                OnRequest();
                CoroutineManager.Run(WaitForConnection(onSuccess, onFail));
                return;
            }

            IsConnected = true;
            onSuccess();
        }

        public void Login(Action onSuccess, Action onFail = null) {
            Debug.Log("Server.Login()");
            new DeviceAuthenticationRequest().Send((response) => {
                    OnResponse();

                    if (response.HasErrors) {
                        if (onFail != null) onFail();
                        return;
                    }

                    onSuccess();
            });
            OnRequest();
        }

        public void SavePlayer(PlayerState player,
            Action onSuccess = null, Action onFail = null) {
            Debug.LogFormat("Server.SavePlayer()");
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

        public void GetPlayer(
            Action<PlayerState> onSuccess, Action onFail = null) {
            Debug.LogFormat("Server.GetPlayer()");
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

        public void Dispose() {
            Debug.Log("Server.Dispose()");
            GS.ShutDown();
            IsConnected = false;
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
            EventBus.Dispatch(Event.Request, this);
        }

        private void OnResponse() {
            --PendingRequestsCount;
            EventBus.Dispatch(Event.Response, this);
        }
    }
}
