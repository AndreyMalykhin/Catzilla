using UnityEngine;
using System;
using System.Globalization;
using System.Collections;
using System.Text;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.Model;

namespace Catzilla.PlayerModule.Model {
    [CreateAssetMenuAttribute]
    public class PlayerStateStorage: ScriptableObject {
        public enum Event {Save}

        [Inject]
        [NonSerialized]
        protected EventBus EventBus;

        [NonSerialized]
        protected PlayerState Player;

        [Inject("SecretKey")]
        [NonSerialized]
        private byte[] secretKey;

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
                    string serializedPlayer = SecurityUtils.Decrypt(
                        PlayerPrefs.GetString("Player"), secretKey);
                    Player =
                        JsonUtility.FromJson<PlayerState>(serializedPlayer);
                    DebugUtils.Log(
                        "PlayerStateStorage.Get(); playe={0}", Player);
                }
            }

            return Player;
        }

        public virtual void Save(PlayerState player) {
            Player = player;
            Player.SaveDate = DateTime.UtcNow;
            string serializedPlayer =
                SecurityUtils.Encrypt(JsonUtility.ToJson(Player), secretKey);
            PlayerPrefs.SetString("Player", serializedPlayer);
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
