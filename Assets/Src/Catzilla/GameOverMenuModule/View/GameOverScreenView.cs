using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using DG.Tweening;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;

namespace Catzilla.GameOverMenuModule.View {
    public class GameOverScreenView: strange.extensions.mediation.impl.View {
        public enum Event {Show, Hide}

        [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
        public IEventDispatcher EventBus {get; set;}

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
                    EventBus.Dispatch(Event.Show);
                });
        }

        public void Hide() {
            Menu.Hide();
            canvas.enabled = false;
            FadeOut();
            EventBus.Dispatch(Event.Hide);
        }

        private void FadeOut() {
            Color currentColor = image.color;
            image.color =
                new Color(currentColor.r, currentColor.g, currentColor.b, 0f);
        }
    }
}
