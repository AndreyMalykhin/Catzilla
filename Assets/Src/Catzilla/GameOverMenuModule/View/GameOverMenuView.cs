using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.Util;

namespace Catzilla.GameOverMenuModule.View {
    public class GameOverMenuView: strange.extensions.mediation.impl.View {
        public enum Event {
            ExitBtnClick,
            RestartBtnClick,
            ResurrectBtnClick,
            RewardBtnClick,
            LeaderboardBtnClick
        }

        [Inject]
        public EventBus EventBus {get; set;}

        public int AvailableResurrectionsCount {
            get {
                return availableResurrectionsCount;
            }
            set {
                availableResurrectionsCount = value;
                RenderResurrectBtn();
            }
        }

        public string ResurrectTextTemplate {
            get {return resurrectTextTemplate;}
            set {resurrectTextTemplate = value;}
        }

        public Text RestartText;
        public Text ExitText;
        public Text ResurrectText;
        public Text RewardText;
        public Text LeaderboardText;
        public Button ExitBtn;
        public Button RestartBtn;
        public Button ResurrectBtn;
        public Button RewardBtn;
        public Button LeaderboardBtn;

        private Canvas canvas;
        private int availableResurrectionsCount;
        private string resurrectTextTemplate = "Resurrect ({0})";

        [PostConstruct]
        public void OnConstruct() {
            // DebugUtils.Log("GameOverMenuView.OnConstruct()");
            canvas = GetComponent<Canvas>();
            canvas.enabled = false;
            ExitBtn.onClick.AddListener(() => {
                EventBus.Fire(Event.ExitBtnClick, new Evt(this));
            });
            RestartBtn.onClick.AddListener(() => {
                EventBus.Fire(Event.RestartBtnClick, new Evt(this));
            });
            ResurrectBtn.onClick.AddListener(() => {
                EventBus.Fire(Event.ResurrectBtnClick, new Evt(this));
            });
            RewardBtn.onClick.AddListener(() => {
                EventBus.Fire(Event.RewardBtnClick, new Evt(this));
            });
            LeaderboardBtn.onClick.AddListener(() => {
                EventBus.Fire(Event.LeaderboardBtnClick, new Evt(this));
            });
            RenderResurrectBtn();
        }

        public void Show() {
            canvas.enabled = true;
        }

        public void Hide() {
            canvas.enabled = false;
        }

        private void RenderResurrectBtn() {
            ResurrectText.text = string.Format(
                resurrectTextTemplate, availableResurrectionsCount);
            ResurrectBtn.interactable = availableResurrectionsCount > 0;
        }
    }
}
