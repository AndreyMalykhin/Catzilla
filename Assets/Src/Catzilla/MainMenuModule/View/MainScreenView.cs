using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Catzilla.MainMenuModule.View {
    public class MainScreenView: strange.extensions.mediation.impl.View {
        [SerializeField]
        private MainMenuView menu;

        private Canvas canvas;

        [PostConstruct]
        public void OnReady() {
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
