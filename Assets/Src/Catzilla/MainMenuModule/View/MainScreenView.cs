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
    }
}
