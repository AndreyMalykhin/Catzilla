using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;

namespace Catzilla.MainMenuModule.View {
    public class MainScreenView: MonoBehaviour {
        public Button StartBtn;
        public Button ExitBtn;
        public Button LeaderboardBtn;
        public Button AchievementsBtn;
        public Button FeedbackBtn;
        public Button ReplaysBtn;
        public Button LoginBtn {get {return loginBtn;}}

        [SerializeField]
        private Button loginBtn;
    }
}
