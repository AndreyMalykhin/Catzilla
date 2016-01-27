using UnityEngine;
using System;
using System.Collections;
using Catzilla.CommonModule.Model;

namespace Catzilla.PlayerModule.Model {
    public class PlayerStateStorage {
        private PlayerState player;

        public PlayerState Get() {
            if (player == null) {
                player = new PlayerState{
                    Level = PlayerPrefs.GetInt("Level"),
                    ScoreRecord = PlayerPrefs.GetInt("ScoreRecord")
                };
            }

            return player;
        }

        public void Save(PlayerState player) {
            this.player = player;
            PlayerPrefs.SetInt("Level", player.Level);
            PlayerPrefs.SetInt("ScoreRecord", player.ScoreRecord);
            PlayerPrefs.Save();
        }

        public void Sync(
            Server server, Action onSuccess = null, Action onFail = null) {
            Debug.Log("PlayerStateStorage.Sync()");
            server.GetPlayer(
                (remotePlayer) => {
                    PlayerState localPlayer = Get();

                    if (remotePlayer != null) {
                        localPlayer.Level =
                            Mathf.Max(localPlayer.Level, remotePlayer.Level);
                        localPlayer.ScoreRecord = Mathf.Max(
                            localPlayer.ScoreRecord, remotePlayer.ScoreRecord);
                    }

                    server.SavePlayer(localPlayer, onSuccess, onFail);
                },
                onFail
            );
        }
    }
}
