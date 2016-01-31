using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;

namespace Catzilla.GameOverMenuModule.View {
    public class GameOverMenuView: strange.extensions.mediation.impl.View {
        public enum Event {
            ExitBtnClick,
            RestartBtnClick,
            ResurrectBtnClick,
            RewardBtnClick,
            LeaderboardBtnClick
        }

        [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
        public IEventDispatcher EventBus {get; set;}

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
                EventBus.Dispatch(Event.ExitBtnClick);
            });
            RestartBtn.onClick.AddListener(() => {
                EventBus.Dispatch(Event.RestartBtnClick);
            });
            ResurrectBtn.onClick.AddListener(() => {
                EventBus.Dispatch(Event.ResurrectBtnClick);
            });
            RewardBtn.onClick.AddListener(() => {
                EventBus.Dispatch(Event.RewardBtnClick);
            });
            LeaderboardBtn.onClick.AddListener(() => {
                EventBus.Dispatch(Event.LeaderboardBtnClick);
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
