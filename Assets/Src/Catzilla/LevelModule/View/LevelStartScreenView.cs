using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.Util;

namespace Catzilla.LevelModule.View {
    public class LevelStartScreenView: strange.extensions.mediation.impl.View {
        public Text Msg;
        public float HideDelay = 1f;

        private Canvas canvas;

        [PostConstruct]
        public void OnConstruct() {
            canvas = GetComponent<Canvas>();
            canvas.enabled = false;
        }

        public void Show() {
            canvas.enabled = true;
        }

        public void Hide(Action onDone = null) {
            StartCoroutine(DoHide(onDone));
        }

        private IEnumerator DoHide(Action onDone = null) {
            yield return new WaitForSecondsRealtime(HideDelay);
            canvas.enabled = false;

            if (onDone != null) {
                onDone();
            }
        }
    }
}
