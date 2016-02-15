using UnityEngine;
using System;
using System.Globalization;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.Model;

namespace Catzilla.PlayerModule.Model {
    // TODO encryption
    [CreateAssetMenuAttribute]
    public class PlayerStateStorage: ScriptableObject {
        public enum Event {Save}

        [Inject]
        public EventBus EventBus {get; set;}

        [NonSerialized]
        protected PlayerState Player;

        [NonSerialized]
        private PlayerState cachedRemotePlayer;

        [NonSerialized]
        private bool isPlayerExists;

        [PostInject]
        public void OnConstruct() {
            isPlayerExists = PlayerPrefs.HasKey("Player");
        }

        public virtual PlayerState Get() {
            if (Player == null) {
                if (isPlayerExists) {
                    Player = JsonUtility.FromJson<PlayerState>(
                        PlayerPrefs.GetString("Player"));
                    DebugUtils.Log(
                        "PlayerStateStorage.Get(); playe={0}", Player);
                }
            }

            return Player;
        }

        public virtual void Save(PlayerState player) {
            Player = player;
            Player.SaveDate = DateTime.UtcNow;
            PlayerPrefs.SetString("Player", JsonUtility.ToJson(Player));
            PlayerPrefs.Save();
            DebugUtils.Log("PlayerStateStorage.Save(); player={0}", Player);
            EventBus.Fire(Event.Save, new Evt(this));
        }

        public virtual void Sync(
            Server server, Action onSuccess = null, Action onFail = null) {
            DebugUtils.Log("PlayerStateStorage.Sync()");
            PlayerState localPlayer = Get();
            DebugUtils.Assert(localPlayer != null);

            if (cachedRemotePlayer != null) {
                server.SavePlayer(localPlayer, onSuccess, onFail);
                return;
            }

            server.GetPlayer(
                (PlayerState remotePlayer) => {
                    cachedRemotePlayer = remotePlayer;

                    if (remotePlayer != null
                        && remotePlayer.PlayTime > localPlayer.PlayTime) {
                        Player = remotePlayer;
                        if (onSuccess != null) onSuccess();
                        return;
                    }

                    server.SavePlayer(localPlayer, onSuccess, onFail);
                },
                onFail
            );
        }
    }
}
