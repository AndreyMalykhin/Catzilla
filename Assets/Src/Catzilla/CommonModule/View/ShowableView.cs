using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;

namespace Catzilla.CommonModule.View {
    public class ShowableView: MonoBehaviour, IPoolable {
        public Action<ShowableView> OnShow {get; set;}
        public Action<ShowableView> OnHide {get; set;}

        [Tooltip("In seconds")]
        public float AutoHideDelay;

        private static readonly int isShownParam =
            Animator.StringToHash("IsShown");

        [SerializeField]
        private Animator animator;

        [SerializeField]
        private Canvas canvas;

        [SerializeField]
        private bool isShowAnimated;

        [SerializeField]
        private bool isHideAnimated;

        private IEnumerator autoHideCoroutine;

        public void Show() {
            if (canvas.enabled) {
                return;
            }

            canvas.enabled = true;

            if (AutoHideDelay > 0f) {
                autoHideCoroutine = AutoHide();
                StartCoroutine(autoHideCoroutine);
            }

            if (animator != null) {
                animator.SetBool(isShownParam, true);
            }

            if (!isShowAnimated) {
                DoShow();
            }
        }

        public void Hide() {
            if (!canvas.enabled) {
                return;
            }

            if (autoHideCoroutine != null) {
                StopCoroutine(autoHideCoroutine);
            }

            if (animator != null) {
                animator.SetBool(isShownParam, false);
            }

            if (!isHideAnimated) {
                DoHide();
            }
        }

        void IPoolable.Reset() {
            canvas.enabled = false;
            OnHide = null;
            OnShow = null;

            if (animator != null) {
                animator.SetBool(isShownParam, false);
            }

            if (autoHideCoroutine != null) {
                StopCoroutine(autoHideCoroutine);
            }
        }

        private void OnShowAnimationEnd() {
            if (!isShowAnimated) {
                return;
            }

            DoShow();
        }

        private void OnHideAnimationEnd() {
            if (!isHideAnimated) {
                return;
            }

            DoHide();
        }

        private void DoShow() {
            if (OnShow != null) OnShow(this);
        }

        private void DoHide() {
            canvas.enabled = false;
            if (OnHide != null) OnHide(this);
        }

        private void Awake() {
            canvas.enabled = false;
        }

        private IEnumerator AutoHide() {
            yield return new WaitForSecondsRealtime(AutoHideDelay);
            Hide();
        }
    }
}
