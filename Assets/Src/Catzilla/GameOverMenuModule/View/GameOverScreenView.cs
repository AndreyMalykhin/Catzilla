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
        public void OnReady() {
            canvas = GetComponent<Canvas>();
            canvas.enabled = false;
            image = GetComponent<Image>();
            Color currentColor = image.color;
            image.color =
                new Color(currentColor.r, currentColor.g, currentColor.b, 0f);
        }

        public void Show() {
            canvas.enabled = true;
            image.DOFade(1f, animationDuration)
                .SetUpdate(UpdateType.Normal, true)
                .OnComplete(() => {
                    menu.Show();
                    EventBus.Dispatch(Event.Show);
                });
        }

        public void Hide() {
            menu.Hide();
            image.DOFade(0f, animationDuration)
                .SetUpdate(UpdateType.Normal, true)
                .OnComplete(() => {
                    canvas.enabled = false;
                    EventBus.Dispatch(Event.Hide);
                });
        }
    }
}
