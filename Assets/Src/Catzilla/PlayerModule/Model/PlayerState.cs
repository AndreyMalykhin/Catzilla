using UnityEngine;
using System;
using System.Collections;
using Catzilla.CommonModule.Util;

namespace Catzilla.PlayerModule.Model {
    [System.Serializable]
    public class PlayerState: ISerializationCallbackReceiver {
        public int Level;
        public int ScoreRecord;
        public int AvailableResurrectionsCount = 1;
        public DateTime SaveDate;
        public DateTime LastSeenDate;

        [SerializeField]
        private long serializedSaveDate;

        [SerializeField]
        private long serializedLastSeenDate;

        public void OnBeforeSerialize() {
            // DebugUtils.Log(
            //     "PlayerState.OnBeforeSerialize(); saveDate={0}", SaveDate);
            serializedSaveDate = SaveDate.ToBinary();
            serializedLastSeenDate = LastSeenDate.ToBinary();
        }

        public void OnAfterDeserialize() {
            SaveDate = DateTime.FromBinary(serializedSaveDate);
            LastSeenDate = DateTime.FromBinary(serializedLastSeenDate);
            // DebugUtils.Log(
            //     "PlayerState.OnAfterDeserialize(); saveDate={0}", SaveDate);
        }

        public override string ToString() {
            return string.Format(
                "level={0}; scoreRecord={1}; availableResurrectionsCount={2}; saveDate={3}; lastSeenDate={4}",
                Level, ScoreRecord, AvailableResurrectionsCount, SaveDate, LastSeenDate);
        }
    }
}
