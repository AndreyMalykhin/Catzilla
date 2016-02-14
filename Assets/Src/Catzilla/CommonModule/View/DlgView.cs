using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Catzilla.CommonModule.View {
    public class DlgView: MonoBehaviour {
        public Text Msg;
        public Button OkBtn;

        private Canvas canvas;

        public void Show() {
            canvas.enabled = true;
        }

        public void Hide() {
            canvas.enabled = false;
        }

        private void Awake() {
            canvas = GetComponent<Canvas>();
            canvas.enabled = false;
            OkBtn.onClick.AddListener(OnOkBtnClick);
        }

        private void OnOkBtnClick() {
            Hide();
        }
    }
}
