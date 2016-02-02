using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using DG.Tweening;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.Util;

namespace Catzilla.GameOverMenuModule.View {
    public class GameOverScreenView: strange.extensions.mediation.impl.View {
        public enum Event {Show, Hide}

        [Inject]
        public EventBus EventBus {get; set;}

        public GameOverMenuView Menu;

        [SerializeField]
        private float animationDuration = 1f;

        private Canvas canvas;
        private Image image;

        [PostConstruct]
        public void OnConstruct() {
            canvas = GetComponent<Canvas>();
            canvas.enabled = false;
            image = GetComponent<Image>();
            FadeOut();
        }

        public void Show(Action onDone) {
            canvas.enabled = true;
            image.DOFade(1f, animationDuration)
                .SetUpdate(UpdateType.Normal, true)
                .OnComplete(() => {
                    Menu.Show();
                    onDone();
                    EventBus.Fire(Event.Show, new Evt(this));
                });
        }

        public void Hide() {
            Menu.Hide();
            canvas.enabled = false;
            FadeOut();
            EventBus.Fire(Event.Hide, new Evt(this));
        }

        private void FadeOut() {
            Color currentColor = image.color;
            image.color =
                new Color(currentColor.r, currentColor.g, currentColor.b, 0f);
        }
    }
}
