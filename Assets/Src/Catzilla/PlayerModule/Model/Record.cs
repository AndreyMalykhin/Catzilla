using UnityEngine;
using System;

namespace Catzilla.PlayerModule.Model {
    [Serializable]
    public class Record {
        public string Id {
            get {return id;}
            private set {id = value;}
        }

        public int Value {
            get {return value;}
            set {
                if (this.value >= value) {
                    return;
                }

                this.value = value;
                isSynced = false;
            }
        }

        public bool IsSynced {
            get {return isSynced;}
            set {isSynced = value;}
        }

        [SerializeField]
        private string id;

        [SerializeField]
        private int value;

        [SerializeField]
        private bool isSynced;

        public Record(string id) {
            this.id = id;
        }

        public override string ToString() {
            return string.Format(
                "id={0}; value={1}; isSynced={2}", id, value, isSynced);
        }
    }
}
