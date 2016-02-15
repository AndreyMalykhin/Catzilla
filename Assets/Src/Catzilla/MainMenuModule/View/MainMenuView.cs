using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;

namespace Catzilla.MainMenuModule.View {
    public class MainMenuView: MonoBehaviour {
        public enum Event {
            ExitBtnClick,
            StartBtnClick,
            LeaderboardBtnClick,
            AchievementsBtnClick
        }

        [Inject]
        public EventBus EventBus {get; set;}

        public Button StartBtn;
        public Button ExitBtn;
        public Button LeaderboardBtn;
        public Button AchievementsBtn;

        private Canvas canvas;

        [PostInject]
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
            AchievementsBtn.onClick.AddListener(() => {
                EventBus.Fire(Event.AchievementsBtnClick, new Evt(this));
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
