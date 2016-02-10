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

        [SerializeField]
        private long serializedSaveDate;

        public void OnBeforeSerialize() {
            DebugUtils.Log(
                "PlayerState.OnBeforeSerialize(); saveDate={0}", SaveDate);
            serializedSaveDate = SaveDate.ToBinary();
        }

        public void OnAfterDeserialize() {
            SaveDate = DateTime.FromBinary(serializedSaveDate);
            DebugUtils.Log(
                "PlayerState.OnAfterDeserialize(); saveDate={0}", SaveDate);
        }
    }
}
