using UnityEngine;
using System;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.Model;

namespace Catzilla.PlayerModule.Model {
    [CreateAssetMenuAttribute]
    public class PlayerStateStorageStub: PlayerStateStorage {
        [SerializeField]
        private int level;

        [SerializeField]
        private int scoreRecord;

        [SerializeField]
        private int availableResurrectionsCount = 1;

        [PostInject]
        new public void OnConstruct() {
            DebugUtils.Assert(Debug.isDebugBuild);
        }

        public override PlayerState Get() {
            if (Player == null) {
                Player = new PlayerState{
                    Level = level,
                    ScoreRecord = scoreRecord,
                    AvailableResurrectionsCount =
                        availableResurrectionsCount
                };
            }

            return Player;
        }

        public override void Save(PlayerState player) {
            Player = player;
            Player.SaveDate = DateTime.UtcNow;
            EventBus.Fire(Event.Save, new Evt(this));
        }

        public override void Sync(
            Server server, Action onSuccess = null, Action onFail = null) {
            if (onSuccess != null) onSuccess();
        }
    }
}
