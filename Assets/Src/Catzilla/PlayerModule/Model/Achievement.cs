using UnityEngine;
using System;

namespace Catzilla.PlayerModule.Model {
    [Serializable]
    public class Achievement {
        public string Id {
            get {return id;}
            private set {id = value;}
        }

        public bool IsSynced {
            get {return isSynced;}
            set {isSynced = value;}
        }

        [SerializeField]
        private string id;

        [SerializeField]
        private bool isSynced;

        public Achievement(string id) {
            this.id = id;
        }

        public override string ToString() {
            return string.Format("id={0}; isSynced={1}", id, isSynced);
        }
    }
}
