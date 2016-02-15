using UnityEngine;
using System;
using System.Text;
using System.Collections.Generic;
using Catzilla.CommonModule.Util;

namespace Catzilla.PlayerModule.Model {
    [Serializable]
    public class PlayerState: ISerializationCallbackReceiver {
        public List<Achievement> Achievements {get {return achievements;}}

        public bool IsScoreSynced {
            get {return isScoreSynced;}
            set {isScoreSynced = value;}
        }

        public int Level {
            get {return level;}
            set {level = value;}
        }

        public int AvailableResurrectionsCount {
            get {return availableResurrectionsCount;}
            set {availableResurrectionsCount = value;}
        }

        public DateTime SaveDate {
            get {return saveDate;}
            set {saveDate = value;}
        }

        public DateTime LastSeenDate {
            get {return lastSeenDate;}
            set {lastSeenDate = value;}
        }

        public TimeSpan PlayTime {
            get {return playTime;}
            set {playTime = value;}
        }

        public int ScoreRecord {
            get {return scoreRecord;}
            set {
                if (value <= scoreRecord) {
                    return;
                }

                scoreRecord = value;
                isScoreSynced = false;
            }
        }

        [SerializeField]
        private List<Achievement> achievements = new List<Achievement>(8);

        [SerializeField]
        private bool isScoreSynced;

        [SerializeField]
        private int availableResurrectionsCount = 1;

        [SerializeField]
        private int level;

        [SerializeField]
        private int scoreRecord;

        [SerializeField]
        private long serializedSaveDate;

        [SerializeField]
        private long serializedLastSeenDate;

        [SerializeField]
        private long serializedPlayTime;

        [NonSerialized]
        private DateTime lastSeenDate;

        [NonSerialized]
        private TimeSpan playTime;

        [NonSerialized]
        private DateTime saveDate;

        public void AddAchievement(Achievement achievement) {
            achievements.Add(achievement);
        }

        public void OnBeforeSerialize() {
            // DebugUtils.Log(
            //     "PlayerState.OnBeforeSerialize(); saveDate={0}", SaveDate);
            serializedSaveDate = saveDate.ToBinary();
            serializedLastSeenDate = lastSeenDate.ToBinary();
            serializedPlayTime = playTime.Ticks;
        }

        public void OnAfterDeserialize() {
            saveDate = DateTime.FromBinary(serializedSaveDate);
            lastSeenDate = DateTime.FromBinary(serializedLastSeenDate);
            playTime = new TimeSpan(serializedPlayTime);
            // DebugUtils.Log(
            //     "PlayerState.OnAfterDeserialize(); saveDate={0}", SaveDate);
        }

        public override string ToString() {
            StringBuilder strBuilder = new StringBuilder(64);

            for (int i = 0; i < achievements.Count; ++i) {
                strBuilder.Append(achievements[i]).Append(", ");
            }

            return string.Format(
                "level={0}; scoreRecord={1}; availableResurrectionsCount={2}; saveDate={3}; lastSeenDate={4}; playTime={5}; isScoreSynced={6}, achievements={7}",
                level, scoreRecord, availableResurrectionsCount, saveDate, lastSeenDate, playTime, isScoreSynced, strBuilder.ToString());
        }
    }
}
