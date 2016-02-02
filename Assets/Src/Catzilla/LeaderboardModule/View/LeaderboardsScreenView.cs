using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.Util;

namespace Catzilla.LeaderboardModule.View {
    public class LeaderboardsScreenView
        : strange.extensions.mediation.impl.View {
        public enum Event {BackBtnClick}

        [Inject]
        public EventBus EventBus {get; set;}

        public ScoreLeaderboardView ScoreLeaderboard;

        [SerializeField]
        private Button backBtn;

        private Canvas canvas;

        [PostConstruct]
        public void OnConstruct() {
            canvas = GetComponent<Canvas>();
            canvas.enabled = false;
            backBtn.onClick.AddListener(OnBackBtnClick);
        }

        public void Show() {
            canvas.enabled = true;
        }

        public void Hide() {
            canvas.enabled = false;
        }

        private void OnBackBtnClick() {
            EventBus.Fire(Event.BackBtnClick, new Evt(this));
        }
    }
}
