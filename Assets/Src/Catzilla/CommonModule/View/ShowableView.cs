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
        private DeactivatableView deactivatable;

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
        [Tooltip("In seconds")]
        private float autoHideDelay;

        [SerializeField]
        private bool isAutoHideUnscaled;

        private bool isShown;
        private IEnumerator unscaledAutoHider;
        private WaitForSecondsRealtime autoHideDelayWaiter;

        [PostInject]
        public void OnConstruct() {
            autoHideDelayWaiter = new WaitForSecondsRealtime(autoHideDelay);
            GameObjectUtils.SetActive(gameObject, isShown, deactivatable);
        }

        public void Show() {
            // DebugUtils.Log("ShowableView.Show()");
            if (isShown) {
                return;
            }

            if (OnPreShow != null) OnPreShow(this);
            eventBus.Fire((int) Events.ShowablePreShow, new Evt(this));
            isShown = true;
            GameObjectUtils.SetActive(gameObject, isShown, deactivatable);

            if (autoHideDelay > 0f) {
                if (isAutoHideUnscaled) {
                    unscaledAutoHider = UnscaledAutoHider();
                    StartCoroutine(unscaledAutoHider);
                } else {
                    Invoke("Hide", autoHideDelay);
                }
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

            StopAutoHide();

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

            StopAutoHide();
            unscaledAutoHider = null;
            GameObjectUtils.SetActive(gameObject, isShown, deactivatable);
        }

		void IPoolable.OnTake() {}

        private void StopAutoHide() {
            if (isAutoHideUnscaled) {
                if (unscaledAutoHider != null) {
                    StopCoroutine(unscaledAutoHider);
                }
            } else {
                CancelInvoke("Hide");
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
            GameObjectUtils.SetActive(gameObject, isShown, deactivatable);
            if (OnHide != null) OnHide(this);
        }

        private void FireShowEvent() {
            if (OnShow != null) OnShow(this);
            eventBus.Fire((int) Events.ShowableShow, new Evt(this));
        }

        private IEnumerator UnscaledAutoHider() {
            autoHideDelayWaiter.Restart();
            yield return autoHideDelayWaiter;
            Hide();
        }
    }
}
