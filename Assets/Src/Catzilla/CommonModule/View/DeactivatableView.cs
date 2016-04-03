using UnityEngine;

namespace Catzilla.CommonModule.View {
    public class DeactivatableView: MonoBehaviour {
        public virtual bool IsActive {
            get {return gameObject.activeInHierarchy;}
            set {gameObject.SetActive(value);}
        }
    }
}
