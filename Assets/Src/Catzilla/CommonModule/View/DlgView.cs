using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Catzilla.CommonModule.View {
    public class DlgView: strange.extensions.mediation.impl.View {
        public Button OkBtn;

        private Canvas canvas;

        [PostConstruct]
        public void OnConstruct() {
            canvas = GetComponent<Canvas>();
            canvas.enabled = false;
            OkBtn.onClick.AddListener(OnOkBtnClick);
        }

        public void Show() {
            canvas.enabled = true;
        }

        public void Hide() {
            canvas.enabled = false;
        }

        private void OnOkBtnClick() {
            Hide();
        }
    }
}
