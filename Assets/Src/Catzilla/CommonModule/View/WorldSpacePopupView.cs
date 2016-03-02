using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;

namespace Catzilla.CommonModule.View {
    public class WorldSpacePopupView: MonoBehaviour, IPoolable {
        public Camera LookAtTarget {
            get {return lookAtTarget;}
            set {
                lookAtTarget = value;
                FitIntoCamera();
            }
        }

        public Text Msg;
        public float Speed = 16f;
        public float Offset;

        private Camera lookAtTarget;
        private readonly Vector3[] corners = new Vector3[4];

        public void PlaceAbove(Bounds bounds) {
            transform.position = new Vector3(
                bounds.center.x, bounds.max.y + Offset, bounds.center.z);
            FitIntoCamera();
        }

        void IPoolable.Reset() {
            LookAtTarget = null;
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
            if (LookAtTarget != null) {
                transform.rotation =
                    Quaternion.LookRotation(LookAtTarget.transform.forward);
            }

            transform.Translate(0f, Speed * Time.deltaTime, 0f);
        }
    }
}
