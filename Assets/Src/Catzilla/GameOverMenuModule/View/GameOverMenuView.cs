using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;

namespace Catzilla.GameOverMenuModule.View {
    public class GameOverMenuView: strange.extensions.mediation.impl.View {
        public enum Event {ExitBtnClick, RestartBtnClick, ResurrectBtnClick}

        [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
        public IEventDispatcher EventBus {get; set;}

        public Text RestartText;
        public Text ExitText;
        public Text ResurrectText;

        [SerializeField]
        private Button exitBtn;

        [SerializeField]
        private Button restartBtn;

        [SerializeField]
        private Button resurrectBtn;

        private Canvas canvas;

        [PostConstruct]
        public void OnConstruct() {
            // Debug.Log("GameOverMenuView.OnConstruct()");
            canvas = GetComponent<Canvas>();
            canvas.enabled = false;
            exitBtn.onClick.AddListener(() => {
                EventBus.Dispatch(Event.ExitBtnClick);
            });
            restartBtn.onClick.AddListener(() => {
                EventBus.Dispatch(Event.RestartBtnClick);
            });
            resurrectBtn.onClick.AddListener(() => {
                EventBus.Dispatch(Event.ResurrectBtnClick);
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
