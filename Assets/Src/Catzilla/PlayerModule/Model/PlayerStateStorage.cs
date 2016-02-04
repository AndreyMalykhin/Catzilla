using UnityEngine;
using System;
using System.Collections;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.context.api;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.Model;

namespace Catzilla.PlayerModule.Model {
    [CreateAssetMenuAttribute]
    public class PlayerStateStorage: ScriptableObject {
        public enum Event {Save}

        [Inject]
        public EventBus EventBus {get; set;}

        protected PlayerState Player;

        public virtual PlayerState Get() {
            if (Player == null) {
                bool isPlayerExists = PlayerPrefs.HasKey("Level");

                if (isPlayerExists) {
                    Player = new PlayerState{
                        Level = PlayerPrefs.GetInt("Level"),
                        ScoreRecord = PlayerPrefs.GetInt("ScoreRecord"),
                        AvailableResurrectionsCount =
                            PlayerPrefs.GetInt("AvailableResurrectionsCount")
                    };
                }
            }

            return Player;
        }

        public virtual void Save(PlayerState player) {
            // DebugUtils.Log("PlayerStateStorage.Save()");
            Player = player;
            PlayerPrefs.SetInt("Level", Player.Level);
            PlayerPrefs.SetInt("ScoreRecord", Player.ScoreRecord);
            PlayerPrefs.SetInt("AvailableResurrectionsCount",
                Player.AvailableResurrectionsCount);
            PlayerPrefs.Save();
            EventBus.Fire(Event.Save, new Evt(this));
        }

        public virtual void Sync(
            Server server, Action onSuccess = null, Action onFail = null) {
            // DebugUtils.Log("PlayerStateStorage.Sync()");
            server.GetPlayer(
                (remotePlayer) => {
                    PlayerState localPlayer = Get();
                    DebugUtils.Assert(localPlayer != null);

                    if (remotePlayer != null) {
                        localPlayer.Level =
                            Mathf.Max(localPlayer.Level, remotePlayer.Level);
                        localPlayer.ScoreRecord = Mathf.Max(
                            localPlayer.ScoreRecord, remotePlayer.ScoreRecord);
                        localPlayer.AvailableResurrectionsCount = Mathf.Max(
                            localPlayer.AvailableResurrectionsCount,
                            remotePlayer.AvailableResurrectionsCount);
                    }

                    server.SavePlayer(localPlayer, onSuccess, onFail);
                },
                onFail
            );
        }
    }
}
