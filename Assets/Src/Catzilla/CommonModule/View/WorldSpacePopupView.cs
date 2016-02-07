using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using Catzilla.CommonModule.Util;

namespace Catzilla.CommonModule.View {
    public class WorldSpacePopupView: MonoBehaviour, IPoolable {
        public Text Msg;
        public float Lifetime = 5f;
        public float Speed = 10f;
        public float Offset = 0f;
        public Action<WorldSpacePopupView> OnHide {get; set;}

        public Camera LookAtTarget {
            get {
                return lookAtTarget;
            }
            set {
                lookAtTarget = value;
                FitIntoCamera();
            }
        }

        private Canvas canvas;
        private Camera lookAtTarget;
        private readonly Vector3[] corners = new Vector3[4];

        public void Show() {
            if (canvas.enabled) {
                return;
            }

            canvas.enabled = true;
            Invoke("Hide", Lifetime);
        }

        public void Hide() {
            if (!canvas.enabled) {
                return;
            }

            canvas.enabled = false;
            CancelInvoke("Hide");

            if (OnHide != null) {
                OnHide(this);
            }
        }

        public void PlaceAbove(Bounds bounds) {
            transform.position = new Vector3(
                bounds.center.x, bounds.max.y + Offset, bounds.center.z);
            FitIntoCamera();
        }

        public void Reset() {
            canvas.enabled = false;
            LookAtTarget = null;
            OnHide = null;
        }

        private void FitIntoCamera() {
            if (lookAtTarget == null) {
                return;
            }

            var transform = (RectTransform) this.transform;
            transform.GetWorldCorners(corners);
            Vector3 popupRight = corners[2];
            Vector3 popupLeft = corners[0];
            float distance =
                lookAtTarget.transform.position.y - transform.position.y;
            Vector3 screenRight = lookAtTarget.ViewportToWorldPoint(
                new Vector3(1f, 1f, distance));
            Vector3 screenLeft = lookAtTarget.ViewportToWorldPoint(
                new Vector3(0f, 0f, distance));

            if (popupRight.x > screenRight.x) {
                transform.Translate(screenRight.x - popupRight.x, 0f, 0f);
            } else if (popupLeft.x < screenLeft.x) {
                transform.Translate(screenLeft.x - popupLeft.x, 0f, 0f);
            }
        }

        private void LateUpdate() {
            if (!canvas.enabled) {
                return;
            }

            if (LookAtTarget != null) {
                transform.rotation =
                    Quaternion.LookRotation(LookAtTarget.transform.forward);
            }

            transform.Translate(0f, Speed * Time.deltaTime, 0f);
        }

        private void Awake() {
            canvas = GetComponent<Canvas>();
            canvas.enabled = false;
        }
    }
}
