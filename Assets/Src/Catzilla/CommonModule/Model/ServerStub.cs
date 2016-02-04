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
    public class ServerStub: Server {
        public override void OnConstruct() {
            DebugUtils.Assert(Debug.isDebugBuild);
            GameObject.Find("GameSparks").SetActive(false);
        }

        public override void Connect(Action onSuccess, Action onFail = null) {
            if (onFail != null) onFail();
        }

        public override void Login(
            string name, Action onSuccess, Action onFail = null) {
            DebugUtils.Assert(false);
        }

        public override void LinkFacebookAccount(
            string accessToken, Action onSuccess, Action onFail = null) {
            DebugUtils.Assert(false);
        }

        public override void GetScoreLeaderboard(
            Action<List<ScoreLeaderboardItem>> onSuccess,
            Action onFail = null) {
            DebugUtils.Assert(false);
        }

        public override void SavePlayer(PlayerState player,
            Action onSuccess = null, Action onFail = null) {
            DebugUtils.Assert(false);
        }

        public override void GetPlayer(
            Action<PlayerState> onSuccess, Action onFail = null) {
            DebugUtils.Assert(false);
        }

        public override void Dispose() {
            IsDisposed = true;
            EventBus.Fire(Event.Dispose, new Evt(this));
        }
    }
}
