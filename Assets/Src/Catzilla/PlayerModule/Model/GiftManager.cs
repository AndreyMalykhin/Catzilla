using UnityEngine;
using System;
using Catzilla.CommonModule.Util;

namespace Catzilla.PlayerModule.Model {
    [CreateAssetMenuAttribute]
    public class GiftManager: ScriptableObject {
        [SerializeField]
        private int addResurrectionsCount = 1;

        [Tooltip("In hours")]
        [SerializeField]
        private int playerAbsenceTime = 12;

        /**
         * @return Given resurrections count
         */
        public int Give(PlayerState playerState) {
            // DebugUtils.Log("GiftManager.Give()");
            playerState.AvailableResurrectionsCount += addResurrectionsCount;
            return addResurrectionsCount;
        }

        public bool IsDeserved(PlayerState playerState) {
            return playerState.AvailableResurrectionsCount == 0
                && DateTime.UtcNow >
                    playerState.LastSeenDate.AddHours(playerAbsenceTime);
        }
    }
}
