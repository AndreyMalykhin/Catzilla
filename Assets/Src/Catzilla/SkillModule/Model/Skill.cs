using UnityEngine;
using System;

namespace Catzilla.SkillModule.Model {
    [Serializable]
    public class Skill {
        public int Id {get {return id;}}
        public int BaseId {get {return baseId;}}
        public float Chance {get {return chance;}}
        public float Factor {get {return factor;}}
        public float Duration {get {return duration;}}
        public int Level {get {return level;}}

        [SerializeField]
        private int id;

        [SerializeField]
        private int baseId;

        [SerializeField]
        private int level;

        [SerializeField]
        private float chance;

        [SerializeField]
        private float factor;

        [SerializeField]
        private float duration;
    }
}
