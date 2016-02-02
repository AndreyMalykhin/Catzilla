using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.Util;

namespace Catzilla.MainMenuModule.View {
    public class MainMenuView: strange.extensions.mediation.impl.View {
        public enum Event {ExitBtnClick, StartBtnClick, LeaderboardBtnClick}

        [Inject]
        public EventBus EventBus {get; set;}

        public Button StartBtn;
        public Button ExitBtn;
        public Button LeaderboardBtn;

        private Canvas canvas;

        [PostConstruct]
        public void OnConstruct() {
            // DebugUtils.Log("MainMenuView.OnConstruct()");
            canvas = GetComponent<Canvas>();
            canvas.enabled = false;
            ExitBtn.onClick.AddListener(() => {
                EventBus.Fire(Event.ExitBtnClick, new Evt(this));
            });
            StartBtn.onClick.AddListener(() => {
                EventBus.Fire(Event.StartBtnClick, new Evt(this));
            });
            LeaderboardBtn.onClick.AddListener(() => {
                EventBus.Fire(Event.LeaderboardBtnClick, new Evt(this));
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
