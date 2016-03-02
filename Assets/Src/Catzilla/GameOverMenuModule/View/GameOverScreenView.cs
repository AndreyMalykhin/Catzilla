using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;

namespace Catzilla.GameOverMenuModule.View {
    public class GameOverScreenView: MonoBehaviour {
        public GameOverMenuView Menu {get {return menu;}}

        [SerializeField]
        private GameOverMenuView menu;
    }
}
