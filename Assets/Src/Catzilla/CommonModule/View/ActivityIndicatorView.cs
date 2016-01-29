﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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
            Debug.Log("ActivityIndicatorView.Show()");
            canvas.enabled = true;
            animator.enabled = true;
        }

        public void Hide() {
            Debug.Log("ActivityIndicatorView.Hide()");
            canvas.enabled = false;
            animator.enabled = false;
        }
    }
}
