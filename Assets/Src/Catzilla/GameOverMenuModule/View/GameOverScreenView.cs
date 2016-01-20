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

        [SerializeField]
        private GameOverMenuView menu;

        [SerializeField]
        private float animationDuration = 1f;

        private Canvas canvas;
        private Image image;

        [PostConstruct]
        public void OnConstruct() {
            canvas = GetComponent<Canvas>();
            canvas.enabled = false;
            image = GetComponent<Image>();
            Color currentColor = image.color;
            image.color =
                new Color(currentColor.r, currentColor.g, currentColor.b, 0f);
        }

        public void Show(Action onDone) {
            canvas.enabled = true;
            image.DOFade(1f, animationDuration)
                .SetUpdate(UpdateType.Normal, true)
                .OnComplete(() => {
                    menu.Show();
                    onDone();
                    EventBus.Dispatch(Event.Show);
                });
        }

        public void Hide(Action onDone) {
            menu.Hide();
            var duration = 0f;
            image.DOFade(0f, duration)
                .SetUpdate(UpdateType.Normal, true)
                .OnComplete(() => {
                    canvas.enabled = false;
                    onDone();
                    EventBus.Dispatch(Event.Hide);
                });
        }
    }
}
