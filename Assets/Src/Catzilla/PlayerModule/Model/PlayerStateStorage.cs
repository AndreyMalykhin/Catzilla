using UnityEngine;
using System;
using System.Globalization;
using System.Collections;
using Zenject;
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
                bool isPlayerExists = PlayerPrefs.HasKey("SaveDate");

                if (isPlayerExists) {
                    Player = new PlayerState{
                        Level = PlayerPrefs.GetInt("Level"),
                        ScoreRecord = PlayerPrefs.GetInt("ScoreRecord"),
                        AvailableResurrectionsCount =
                            PlayerPrefs.GetInt("AvailableResurrectionsCount"),
                        SaveDate = DateTime.Parse(
                            PlayerPrefs.GetString("SaveDate"),
                            null,
                            DateTimeStyles.RoundtripKind),
                        LastSeenDate = DateTime.Parse(
                            PlayerPrefs.GetString("LastSeenDate"),
                            null,
                            DateTimeStyles.RoundtripKind)
                    };
                }
            }

            return Player;
        }

        public virtual void Save(PlayerState player) {
            Player = player;
            Player.SaveDate = DateTime.UtcNow;
            PlayerPrefs.SetInt("Level", Player.Level);
            PlayerPrefs.SetInt("ScoreRecord", Player.ScoreRecord);
            PlayerPrefs.SetInt("AvailableResurrectionsCount",
                Player.AvailableResurrectionsCount);
            PlayerPrefs.SetString("SaveDate", Player.SaveDate.ToString("o"));
            PlayerPrefs.SetString(
                "LastSeenDate", Player.LastSeenDate.ToString("o"));
            PlayerPrefs.Save();
            DebugUtils.Log("PlayerStateStorage.Save(); player={0}", player);
            EventBus.Fire(Event.Save, new Evt(this));
        }

        public virtual void Sync(
            Server server, Action onSuccess = null, Action onFail = null) {
            DebugUtils.Log("PlayerStateStorage.Sync()");
            server.GetPlayer(
                (remotePlayer) => {
                    PlayerState localPlayer = Get();
                    DebugUtils.Assert(localPlayer != null);

                    if (remotePlayer != null
                        && (PlayerPrefs.GetInt("WasEverSynced") == 0
                            || remotePlayer.SaveDate > localPlayer.SaveDate)) {
                        localPlayer = remotePlayer;
                        PlayerPrefs.SetInt("WasEverSynced", 1);
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
