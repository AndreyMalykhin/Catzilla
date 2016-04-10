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
        private int availableResurrectionsCount;

        [SerializeField]
        private int availableRewardsCount;

        [SerializeField]
        private int availableSkillPointsCount;

        [SerializeField]
        private bool wasTutorialShown;

        [PostInject]
        new public void OnConstruct() {
            DebugUtils.Assert(Debug.isDebugBuild);
        }

        public override PlayerState Get() {
            if (Player == null) {
                Player = new PlayerState{
                    Level = level,
                    AvailableResurrectionsCount =
                        availableResurrectionsCount,
                    AvailableRewardsCount = availableRewardsCount,
                    AvailableSkillPointsCount = availableSkillPointsCount,
                    WasTutorialShown = wasTutorialShown
                };
            }

            return Player;
        }

        public override void Save(PlayerState player) {
            Player = player;
            Player.SaveDate = DateTime.UtcNow;
            // DebugUtils.Log("PlayerStateStorageStub.Save(); player={0}", Player);
            EventBus.Fire((int) Events.PlayerStateStorageSave, new Evt(this));
        }

        public override void Sync(
            Server server, Action onSuccess = null, Action onFail = null) {
            if (onSuccess != null) onSuccess();
        }
    }
}
