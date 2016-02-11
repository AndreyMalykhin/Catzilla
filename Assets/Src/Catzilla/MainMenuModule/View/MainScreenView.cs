using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;

namespace Catzilla.MainMenuModule.View {
    public class MainScreenView: MonoBehaviour {
        [SerializeField]
        private MainMenuView menu;

        private Canvas canvas;

        [PostInject]
        public void OnConstruct() {
            // DebugUtils.Log("MainScreenView.OnConstruct()");
            canvas = GetComponent<Canvas>();
            canvas.enabled = false;
        }

        public void Show() {
            canvas.enabled = true;
            menu.Show();
        }

        public void Hide() {
            canvas.enabled = false;
            menu.Hide();
        }
    }
}
