using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Catzilla.CommonModule.Util;

namespace Catzilla.CommonModule.View {
    public class ActivityIndicatorView: strange.extensions.mediation.impl.View {
        private Canvas canvas;
        private Animator animator;

        [PostConstruct]
        public void OnConstruct() {
            canvas = GetComponent<Canvas>();
            animator = GetComponent<Animator>();
            canvas.enabled = false;
            animator.enabled = false;
        }

        public void Show() {
            // DebugUtils.Log("ActivityIndicatorView.Show()");
            canvas.enabled = true;
            animator.enabled = true;
        }

        public void Hide() {
            // DebugUtils.Log("ActivityIndicatorView.Hide()");
            canvas.enabled = false;
            animator.enabled = false;
        }
    }
}
