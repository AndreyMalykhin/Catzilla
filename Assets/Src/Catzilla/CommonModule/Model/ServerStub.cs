using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.context.api;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;
using Catzilla.PlayerModule.Model;

namespace Catzilla.CommonModule.Model {
    [CreateAssetMenuAttribute]
    public class ServerStub: Server {
        [PostConstruct]
        public void OnConstruct() {
            DebugUtils.Assert(Debug.isDebugBuild);
        }

        public override void Connect(Action onSuccess, Action onFail = null) {
            onFail();
        }

        public override void Login(Action onSuccess, Action onFail = null) {
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
    }
}
