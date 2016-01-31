using UnityEngine;
using System;
using System.Collections;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.context.api;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.Model;

namespace Catzilla.PlayerModule.Model {
    public class PlayerStateStorage {
        public enum Event {Save}

        [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
        public IEventDispatcher EventBus {get; set;}

        private PlayerState player;

        public PlayerState Get() {
            if (player == null) {
                player = new PlayerState{
                    Level = PlayerPrefs.GetInt("Level", 0),
                    ScoreRecord = PlayerPrefs.GetInt("ScoreRecord", 0),
                    AvailableResurrectionsCount =
                        PlayerPrefs.GetInt("AvailableResurrectionsCount", 1)
                };
            }

            return player;
        }

        public void Save(PlayerState player) {
            // DebugUtils.Log("PlayerStateStorage.Save()");
            this.player = player;
            PlayerPrefs.SetInt("Level", player.Level);
            PlayerPrefs.SetInt("ScoreRecord", player.ScoreRecord);
            PlayerPrefs.SetInt("AvailableResurrectionsCount",
                player.AvailableResurrectionsCount);
            PlayerPrefs.Save();
            EventBus.Dispatch(Event.Save, this);
        }

        public void Sync(
            Server server, Action onSuccess = null, Action onFail = null) {
            // DebugUtils.Log("PlayerStateStorage.Sync()");
            server.GetPlayer(
                (remotePlayer) => {
                    PlayerState localPlayer = Get();

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
