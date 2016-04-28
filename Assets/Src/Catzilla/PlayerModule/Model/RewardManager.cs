using UnityEngine;
using System;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.LevelModule.Model;

namespace Catzilla.PlayerModule.Model {
    [CreateAssetMenuAttribute]
    public class RewardManager: ScriptableObject {
        [Inject]
        [NonSerialized]
        private LevelSettingsStorage levelSettingsStorage;

        [SerializeField]
        private int unlockRewardsCount;

        [Tooltip("In hours")]
        [SerializeField]
        private int lockDuration;

        /**
         * @return Given resurrections count
         */
        public int Give(PlayerState playerState) {
            DebugUtils.Assert(playerState.AvailableRewardsCount > 0);
            int addResurrectionsCount =
                levelSettingsStorage.Get(playerState.Level).ResurrectionReward;
            playerState.AvailableResurrectionsCount += addResurrectionsCount;

            if (--playerState.AvailableRewardsCount == 0) {
                Lock(playerState);
            }

            return addResurrectionsCount;
        }

        /**
         * @return Unlocked rewards count
         */
        public int Unlock(PlayerState playerState) {
            // DebugUtils.Log("RewardManager.Unlock()");
            playerState.AvailableRewardsCount += unlockRewardsCount;
            playerState.RewardUnlockDate = DateTime.MinValue;
            return unlockRewardsCount;
        }

        public bool IsTimeToUnlock(PlayerState playerState) {
            return playerState.AvailableRewardsCount == 0
                && IsLocked(playerState)
                && DateTime.UtcNow >= playerState.RewardUnlockDate;
        }

        private void Lock(PlayerState playerState) {
            playerState.RewardUnlockDate =
                DateTime.UtcNow.AddHours(lockDuration);
        }

        private bool IsLocked(PlayerState playerState) {
            return playerState.RewardUnlockDate != DateTime.MinValue;
        }
    }
}
