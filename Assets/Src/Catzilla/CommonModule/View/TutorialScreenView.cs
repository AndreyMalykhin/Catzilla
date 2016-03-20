using UnityEngine;
using System;
using Zenject;
using Catzilla.CommonModule.Util;

namespace Catzilla.CommonModule.View {
    public class TutorialScreenView: MonoBehaviour {
        public event Action<TutorialScreenView> OnOpen;
        public event Action<TutorialScreenView> OnClose;
        public bool IsOpened {get {return isOpened;}}

        [SerializeField]
        private TutorialStepView[] steps;

        private bool isOpened;
        private int nextStepIndex;

        public void Open() {
            isOpened = true;
            nextStepIndex = 0;
            NextStep();
            if (OnOpen != null) OnOpen(this);
        }

        private void NextStep() {
            steps[nextStepIndex].GetComponent<ShowableView>().Show();
        }

        private void OnNextBtnClick() {
            steps[nextStepIndex].GetComponent<ShowableView>().Hide();
        }

        private void Awake() {
            DebugUtils.Assert(steps.Length != 0);

            for (int i = 0; i < steps.Length; ++i) {
                steps[i].NextBtn.onClick.AddListener(OnNextBtnClick);
                steps[i].GetComponent<ShowableView>().OnHide += OnStepHide;
            }
        }

        private void OnStepHide(ShowableView showable) {
            bool isLastStep = nextStepIndex == steps.Length - 1;

            if (isLastStep) {
                Close();
                return;
            }

            ++nextStepIndex;
            NextStep();
        }

        private void Close() {
            isOpened = false;
            if (OnClose != null) OnClose(this);
        }
    }
}
