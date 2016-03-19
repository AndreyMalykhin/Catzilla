using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;

namespace Catzilla.LevelModule.View {
    public class LevelCompleteScreenView: MonoBehaviour {
        public Button ContinueBtn {get {return continueBtn;}}
        public Button ShareBtn {get {return shareBtn;}}
        public Button WatchReplayBtn {get {return watchReplayBtn;}}
        public Text Score {get {return score;}}
        public RawImage ReplayImg {get {return replayImg;}}
        public GameObject Replay {get {return replay;}}

        [SerializeField]
        private Button continueBtn;

        [SerializeField]
        private Button shareBtn;

        [SerializeField]
        private Text score;

        [SerializeField]
        private RawImage replayImg;

        [SerializeField]
        private Button watchReplayBtn;

        [SerializeField]
        private GameObject replay;
    }
}
