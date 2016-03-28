using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;
using System;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;

namespace Catzilla.CommonModule.View {
    public class ShowableView: MonoBehaviour, IPoolable {
        public bool IsShown {get {return isShown;}}
        public AudioClip ShowSound {get {return showSound;}}
        public AudioClip PreShowSound {get {return preShowSound;}}
        public AudioSource AudioSource {get {return audioSource;}}
        public event Action<ShowableView> OnShow;
        public event Action<ShowableView> OnPreShow;
        public event Action<ShowableView> OnHide;

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

        [SerializeField]
        [FormerlySerializedAs("AutoHideDelay")]
        [Tooltip("In seconds")]
        private float autoHideDelay;

        private bool isShown;
        private IEnumerator autoHider;
        private WaitForSecondsRealtime autoHideDelayWaiter;

        [PostInject]
        public void OnConstruct() {
            autoHideDelayWaiter = new WaitForSecondsRealtime(autoHideDelay);
            gameObject.SetActive(isShown);
        }

        public void Show() {
            // DebugUtils.Log("ShowableView.Show()");
            if (isShown) {
                return;
            }

            if (OnPreShow != null) OnPreShow(this);
            eventBus.Fire((int) Events.ShowablePreShow, new Evt(this));
            isShown = true;
            gameObject.SetActive(isShown);

            if (autoHideDelay > 0f) {
                autoHider = AutoHider();
                StartCoroutine(autoHider);
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

            if (autoHider != null) {
                StopCoroutine(autoHider);
            }

            if (animator != null) {
                animator.SetBool(isShownParam, false);
            }

            if (!isHideAnimated) {
                DoHide();
            }
        }

        void IPoolable.OnReturn() {
            isShown = false;
            OnHide = null;
            OnShow = null;

            if (animator != null && isShown) {
                animator.SetBool(isShownParam, isShown);
            }

            if (autoHider != null) {
                StopCoroutine(autoHider);
                autoHider = null;
            }

            gameObject.SetActive(isShown);
        }

		void IPoolable.OnTake() {}

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
            eventBus.Fire((int) Events.ShowableShow, new Evt(this));
        }

        private IEnumerator AutoHider() {
            autoHideDelayWaiter.Restart();
            yield return autoHideDelayWaiter;
            Hide();
        }
    }
}
