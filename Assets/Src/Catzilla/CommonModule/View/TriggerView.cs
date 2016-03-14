using UnityEngine;
using Catzilla.CommonModule.Util;

namespace Catzilla.CommonModule.View {
    public class TriggerView: MonoBehaviour {
        public TriggerOwnerView Owner {
            get {return owner;}
            set {
                owner = value;
                positionOffset = transform.position - owner.transform.position;
            }
        }

        public Vector3 PositionOffset {get {return positionOffset;}}

        private TriggerOwnerView owner;
        private Vector3 positionOffset;
    }
}
