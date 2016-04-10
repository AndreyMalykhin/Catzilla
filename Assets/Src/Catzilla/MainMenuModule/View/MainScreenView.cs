using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;

namespace Catzilla.MainMenuModule.View {
    public class MainScreenView: MonoBehaviour {
        public Button StartBtn {get {return startBtn;}}
        public Button ExitBtn {get {return exitBtn;}}
        public Button LeaderboardBtn {get {return leaderboardBtn;}}
        public Button AchievementsBtn {get {return achievementsBtn;}}
        public Button FeedbackBtn {get {return feedbackBtn;}}
        public Button ReplaysBtn {get {return replaysBtn;}}
        public Button LoginBtn {get {return loginBtn;}}
        public Button SkillsBtn {get {return skillsBtn;}}

        [SerializeField]
        private Button loginBtn;

        [SerializeField]
        private Button skillsBtn;

        [SerializeField]
        private Button startBtn;

        [SerializeField]
        private Button exitBtn;

        [SerializeField]
        private Button leaderboardBtn;

        [SerializeField]
        private Button achievementsBtn;

        [SerializeField]
        private Button feedbackBtn;

        [SerializeField]
        private Button replaysBtn;
    }
}
