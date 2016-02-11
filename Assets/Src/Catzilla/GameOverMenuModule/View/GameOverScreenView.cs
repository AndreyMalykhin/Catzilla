using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using DG.Tweening;
using Zenject;
using Catzilla.CommonModule.Util;

namespace Catzilla.GameOverMenuModule.View {
    public class GameOverScreenView: MonoBehaviour {
        public enum Event {Show, Hide}

        [Inject]
        public EventBus EventBus {get; set;}

        public GameOverMenuView Menu;

        [SerializeField]
        private float animationDuration = 1f;

        private Canvas canvas;
        private Image image;

        [PostInject]
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
