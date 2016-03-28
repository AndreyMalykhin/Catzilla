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
            set {lookAtTarget = value;}
        }

        public Text Msg;
        public Vector3 Offset;

        [SerializeField]
        private Vector3 offScreenPosition = new Vector3(Int16.MinValue, 0f, 0f);

        private Camera lookAtTarget;
        private readonly Vector3[] corners = new Vector3[4];
        private int countOfUpdatesTillPlacement = -1;
        private Bounds placementTargetBounds;

        public void PlaceAbove(Bounds bounds) {
            placementTargetBounds = bounds;
            countOfUpdatesTillPlacement = 1;
        }

        void IPoolable.OnReturn() {
            LookAtTarget = null;
            countOfUpdatesTillPlacement = -1;
            ResetPosition();
        }

		void IPoolable.OnTake() {}

        private void Awake() {
            ResetPosition();
        }

        private void ResetPosition() {
            transform.position = offScreenPosition;
        }

        private void FitIntoCamera() {
            if (lookAtTarget == null) {
                return;
            }

            var transform = (RectTransform) this.transform;
            transform.GetWorldCorners(corners);
            Vector3 popupRight = corners[2];
            Vector3 popupLeft = corners[0];
            Debug.DrawLine(popupLeft, popupRight);
            float distance =
                lookAtTarget.transform.position.y - transform.position.y;
            Vector3 screenRight = lookAtTarget.ViewportToWorldPoint(
                new Vector3(1f, 1f, distance));
            Vector3 screenLeft = lookAtTarget.ViewportToWorldPoint(
                new Vector3(0f, 0f, distance));

            if (popupLeft.x < screenLeft.x) {
                transform.Translate(screenLeft.x - popupLeft.x, 0f, 0f);
            } else if (popupRight.x > screenRight.x) {
                transform.Translate(screenRight.x - popupRight.x, 0f, 0f);
            }
        }

        private void LateUpdate() {
            if (LookAtTarget == null) {
                return;
            }

            transform.rotation =
                Quaternion.LookRotation(LookAtTarget.transform.forward);
            Place();
        }

        private void Place() {
            if (countOfUpdatesTillPlacement == 0) {
                transform.position = new Vector3(
                    placementTargetBounds.center.x,
                    placementTargetBounds.max.y,
                    placementTargetBounds.center.z) + Offset;
                FitIntoCamera();
                --countOfUpdatesTillPlacement;
            } else if (countOfUpdatesTillPlacement > 0) {
                --countOfUpdatesTillPlacement;
            }
        }
    }
}
