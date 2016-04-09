using UnityEngine;
using System;

namespace Catzilla.SkillModule.Model {
    [Serializable]
    public class BaseSkill {
        public int Id {get {return id;}}
        public SkillType Type {get {return type;}}
        public string Name {get {return name;}}
        public string Description {get {return description;}}
        public Texture2D Img {get {return img;}}

        [SerializeField]
        private int id;

        [SerializeField]
        private SkillType type;

        [SerializeField]
        private Texture2D img;

        [SerializeField]
        private string name;

        [SerializeField]
        private string description;
    }
}
