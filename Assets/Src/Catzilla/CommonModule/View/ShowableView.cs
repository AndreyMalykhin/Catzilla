using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;

namespace Catzilla.CommonModule.View {
    public class ShowableView: MonoBehaviour, IPoolable {
        public enum Event {PreShow, Show}

        public bool IsShown {get {return isShown;}}
        public AudioClip ShowSound {get {return showSound;}}
        public AudioClip PreShowSound {get {return preShowSound;}}
        public AudioSource AudioSource {get {return audioSource;}}
        public event Action<ShowableView> OnShow;
        public event Action<ShowableView> OnPreShow;
        public event Action<ShowableView> OnHide;

        [Tooltip("In seconds")]
        public float AutoHideDelay;

        private static readonly int isShownParam =
            Animator.StringToHash("IsShown");

        [Inject]
        private EventBus eventBus;

        [SerializeField]
        private Animator animator;

        [SerializeField]
        private bool isShowAnimated;

        [SerializeField]
        private bool isHideAnimated;

        [SerializeField]
        private AudioClip showSound;

        [SerializeField]
        private AudioClip preShowSound;

        [SerializeField]
        private AudioSource audioSource;

        private bool isShown;
        private IEnumerator autoHideCoroutine;

        [PostInject]
        public void OnConstruct() {
            gameObject.SetActive(isShown);
        }

        public void Show() {
            // DebugUtils.Log("ShowableView.Show()");
            if (isShown) {
                return;
            }

            if (OnPreShow != null) OnPreShow(this);
            eventBus.Fire(Event.PreShow, new Evt(this));
            isShown = true;
            gameObject.SetActive(isShown);

            if (AutoHideDelay > 0f) {
                autoHideCoroutine = AutoHide();
                StartCoroutine(autoHideCoroutine);
            }

            if (animator != null) {
                animator.SetBool(isShownParam, isShown);
            }

            if (!isShowAnimated) {
                FireShowEvent();
            }
        }

        public void Hide() {
            // DebugUtils.Log("ShowableView.Hide()");
            if (!isShown) {
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
            isShown = false;
            OnHide = null;
            OnShow = null;

            if (animator != null && isShown) {
                animator.SetBool(isShownParam, isShown);
            }

            if (autoHideCoroutine != null) {
                StopCoroutine(autoHideCoroutine);
                autoHideCoroutine = null;
            }
        }

        private void OnShowAnimationEnd() {
            if (!isShowAnimated) {
                return;
            }

            FireShowEvent();
        }

        private void OnHideAnimationEnd() {
            if (!isHideAnimated) {
                return;
            }

            DoHide();
        }

        private void DoHide() {
            isShown = false;
            gameObject.SetActive(isShown);
            if (OnHide != null) OnHide(this);
        }

        private void FireShowEvent() {
            if (OnShow != null) OnShow(this);
            eventBus.Fire(Event.Show, new Evt(this));
        }

        private IEnumerator AutoHide() {
            yield return new WaitForSecondsRealtime(AutoHideDelay);
            Hide();
        }
    }
}
