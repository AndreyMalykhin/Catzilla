using UnityEngine;
using UnityEngine.UI;

namespace Catzilla.CommonModule.View {
    public class UIBlockerView: MonoBehaviour {
        [SerializeField]
        private Canvas canvas;

        public void Show() {
            canvas.enabled = true;
        }

        public void Hide() {
            canvas.enabled = false;
        }

        private void Awake() {
            canvas.enabled = false;
        }
    }
}
