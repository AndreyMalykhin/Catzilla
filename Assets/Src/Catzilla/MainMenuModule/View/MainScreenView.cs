using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;

namespace Catzilla.MainMenuModule.View {
    public class MainScreenView: MonoBehaviour {
        public MainMenuView Menu {get {return menu;}}
        public Button LoginBtn {get {return loginBtn;}}

        [SerializeField]
        private MainMenuView menu;

        [SerializeField]
        private Button loginBtn;

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
