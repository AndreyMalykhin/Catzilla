using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;

namespace Catzilla.MainMenuModule.View {
    public class MainMenuView: strange.extensions.mediation.impl.View {
        public enum Event {ExitBtnClick, StartBtnClick, LeaderboardBtnClick}

        [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
        public IEventDispatcher EventBus {get; set;}

        public Button StartBtn;
        public Button ExitBtn;
        public Button LeaderboardBtn;

        private Canvas canvas;

        [PostConstruct]
        public void OnConstruct() {
            // Debug.Log("MainMenuView.OnConstruct()");
            canvas = GetComponent<Canvas>();
            canvas.enabled = false;
            ExitBtn.onClick.AddListener(() => {
                EventBus.Dispatch(Event.ExitBtnClick);
            });
            StartBtn.onClick.AddListener(() => {
                EventBus.Dispatch(Event.StartBtnClick);
            });
            LeaderboardBtn.onClick.AddListener(() => {
                EventBus.Dispatch(Event.LeaderboardBtnClick);
            });
        }

        public void Show() {
            canvas.enabled = true;
        }

        public void Hide() {
            canvas.enabled = false;
        }
    }
}
