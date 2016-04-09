using UnityEngine;
using System;
using System.Text;
using System.Collections.Generic;
using Catzilla.CommonModule.Util;

namespace Catzilla.PlayerModule.Model {
    [Serializable]
    public class PlayerState: ISerializationCallbackReceiver {
        public List<Achievement> Achievements {get {return achievements;}}
        public List<Record> Records {get {return records;}}
        public List<int> SkillIds {get {return skillIds;}}

        public bool WasTutorialShown {
            get {return wasTutorialShown;}
            set {wasTutorialShown = value;}
        }

        public int Level {
            get {return level;}
            set {level = value;}
        }

        public int AvailableSkillPointsCount {
            get {return availableSkillPointsCount;}
            set {availableSkillPointsCount = value;}
        }

        public int AvailableResurrectionsCount {
            get {return availableResurrectionsCount;}
            set {availableResurrectionsCount = value;}
        }

        public int AvailableRewardsCount {
            get {return availableRewardsCount;}
            set {availableRewardsCount = value;}
        }

        public DateTime RewardUnlockDate {
            get {return rewardUnlockDate;}
            set {rewardUnlockDate = value;}
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

        [SerializeField]
        private List<int> skillIds = new List<int>(8);

        [SerializeField]
        private List<Achievement> achievements = new List<Achievement>(8);

        [SerializeField]
        private List<Record> records = new List<Record>(8);

        [SerializeField]
        private bool wasTutorialShown;

        [SerializeField]
        private int availableResurrectionsCount;

        [SerializeField]
        private int availableRewardsCount = 1;

        [SerializeField]
        private int availableSkillPointsCount;

        [SerializeField]
        private int level;

        [SerializeField]
        private long serializedSaveDate;

        [SerializeField]
        private long serializedLastSeenDate;

        [SerializeField]
        private long serializedRewardUnlockDate;

        [SerializeField]
        private long serializedPlayTime;

        [NonSerialized]
        private DateTime lastSeenDate;

        [NonSerialized]
        private TimeSpan playTime;

        [NonSerialized]
        private DateTime saveDate;

        [NonSerialized]
        private DateTime rewardUnlockDate;

        public void AddSkill(int id) {
            skillIds.Add(id);
        }

        public void RemoveSkill(int id) {
            skillIds.Remove(id);
        }

        public void AddAchievement(Achievement achievement) {
            achievements.Add(achievement);
        }

        public Record GetRecord(string id) {
            Record record = null;

            for (int i = 0; i < records.Count; ++i) {
                record = records[i];

                if (record.Id == id) {
                    return record;
                }
            }

            record = new Record(id);
            records.Add(record);
            return record;
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize() {
            // DebugUtils.Log(
            //     "PlayerState.OnBeforeSerialize(); saveDate={0}", SaveDate);
            serializedSaveDate = saveDate.ToBinary();
            serializedLastSeenDate = lastSeenDate.ToBinary();
            serializedRewardUnlockDate = rewardUnlockDate.ToBinary();
            serializedPlayTime = playTime.Ticks;
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize() {
            saveDate = DateTime.FromBinary(serializedSaveDate);
            lastSeenDate = DateTime.FromBinary(serializedLastSeenDate);
            rewardUnlockDate = DateTime.FromBinary(serializedRewardUnlockDate);
            playTime = new TimeSpan(serializedPlayTime);
            // DebugUtils.Log(
            //     "PlayerState.OnAfterDeserialize(); saveDate={0}", SaveDate);
        }

        public override string ToString() {
            StringBuilder achievementsStr = new StringBuilder(64);

            for (int i = 0; i < achievements.Count; ++i) {
                achievementsStr.Append(achievements[i]).Append(", ");
            }

            StringBuilder recordsStr = new StringBuilder(64);

            for (int i = 0; i < records.Count; ++i) {
                recordsStr.Append(records[i]).Append(", ");
            }

            StringBuilder skillsStr = new StringBuilder(64);

            for (int i = 0; i < skillIds.Count; ++i) {
                skillsStr.Append(skillIds[i]).Append(", ");
            }

            return string.Format(
                "level={0}; availableResurrectionsCount={1}; saveDate={2}; lastSeenDate={3}; playTime={4}; achievements={5}; rewardUnlockDate={6}; availableRewardsCount={7}; wasTutorialShown={8}; records={9}; skills={10}; availableSkillPointsCount={11}",
                level, availableResurrectionsCount, saveDate, lastSeenDate, playTime, achievementsStr.ToString(), rewardUnlockDate, availableRewardsCount, wasTutorialShown, recordsStr.ToString(), skillsStr.ToString(), availableSkillPointsCount);
        }
    }
}
