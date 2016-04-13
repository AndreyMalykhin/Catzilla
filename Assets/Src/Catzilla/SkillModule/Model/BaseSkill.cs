using UnityEngine;
using System;

namespace Catzilla.SkillModule.Model {
    [Serializable]
    public class BaseSkill {
        public int Id {get {return id;}}
        public SkillType Type {get {return type;}}
        public string Name {get {return name;}}
        public string Description {get {return description;}}
        public Sprite Img {get {return img;}}
        public int Order {get {return order;}}

        [SerializeField]
        private int id;

        [SerializeField]
        private SkillType type;

        [SerializeField]
        private Sprite img;

        [SerializeField]
        private string name;

        [SerializeField]
        private string description;

        [SerializeField]
        private int order;
    }
}
