using UnityEngine;
using Zenject;
using Catzilla.CommonModule.Model;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;
using Catzilla.LevelObjectModule.View;
using Catzilla.LevelModule.View;

namespace Catzilla.LevelModule.Controller {
    public class LevelCompleteScreenController {
        [Inject]
        private Game game;

        [Inject]
        private LevelCompleteScreenView screen;

        private bool isVideoRecordingInited;

        [PostInject]
        public void OnConstruct() {
            SetVideoSectionVisible(Everyplay.IsSupported());
        }

        public void OnReadyForVideoRecording(bool isReady) {
            // DebugUtils.Log("LevelCompleteScreenController.OnReadyForVideoRecording()");
            if (isVideoRecordingInited) {
                return;
            }

            isVideoRecordingInited = true;

            if (isReady && Everyplay.IsRecordingSupported()) {
                return;
            }

            SetVideoSectionVisible(false);
        }

        public void OnVideoThumbReady(Texture2D thumb, bool isPortrait) {
            // DebugUtils.Log("LevelCompleteScreenController.OnVideoThumbReady()");
            screen.ReplayImg.texture = thumb;
        }

        public void OnShareBtnClick() {
            Everyplay.ShowSharingModal();
        }

        public void OnContinueBtnClick() {
            var showable = screen.GetComponent<ShowableView>();
            showable.OnHide += OnHide;
            showable.Hide();
        }

        public void OnWatchReplayBtnClick() {
            Everyplay.PlayLastRecording();
        }

        private void OnHide(ShowableView showable) {
            showable.OnHide -= OnHide;
            game.LoadLevel();
        }

        private void SetVideoSectionVisible(bool isVisible) {
            screen.ShareBtn.gameObject.SetActive(isVisible);
            screen.Replay.SetActive(isVisible);
        }
    }
}
