using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;

namespace Catzilla.CommonModule.View {
    public class ScreenSpacePopupView: MonoBehaviour, IPoolable {
        public Action<ScreenSpacePopupView> OnHide {get; set;}
        public Text Msg;
        public float Lifetime = 4f;

        [SerializeField]
        private Canvas canvas;

        private IEnumerator hideLaterCoroutine;

        [PostInject]
        public void OnConstruct() {
            canvas.enabled = false;
        }

        public void Show() {
            if (canvas.enabled) {
                return;
            }

            canvas.enabled = true;
            hideLaterCoroutine = HideLater();
            StartCoroutine(hideLaterCoroutine);
        }

        public void Hide() {
            if (!canvas.enabled) {
                return;
            }

            canvas.enabled = false;
            StopCoroutine(hideLaterCoroutine);

            if (OnHide != null) {
                OnHide(this);
            }
        }

        void IPoolable.Reset() {
            canvas.enabled = false;
            OnHide = null;
        }

        private IEnumerator HideLater() {
            yield return new WaitForSecondsRealtime(Lifetime);
            Hide();
        }
    }
}
