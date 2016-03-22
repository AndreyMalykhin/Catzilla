using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.LevelObjectModule.View;
using Catzilla.PlayerModule.Model;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;

namespace Catzilla.CommonModule.Controller {
    public class EveryplayController {
        [Inject]
        private CoroutineManagerView coroutineManager;

        [Inject]
        private PlayerStateStorage playerStateStorage;

        private readonly Texture2D screenshot =
            new Texture2D(256, 128, TextureFormat.RGBA32, false);

        [PostInject]
        public void OnConstruct() {
            Everyplay.SetDisableSingleCoreDevices(true);
            Everyplay.ReadyForRecording += OnReadyForRecording;
            Everyplay.RecordingStarted += OnRecordingStarted;
        }

        public void OnLevelUnload(Evt evt) {
            Everyplay.StopRecording();
        }

        public void OnPreLevelLoad(Evt evt) {
            Everyplay.StopRecording();
        }

        public void OnPlayerResurrect(Evt evt) {
            Everyplay.ResumeRecording();
        }

        public void OnGameOverScreenShow(ShowableView showable) {
            Everyplay.PauseRecording();
        }

        public void OnLevelStartScreenShow(ShowableView showable) {
            Everyplay.StartRecording();
        }

        public void OnLevelCompleteScreenShow(ShowableView showable) {
            Everyplay.StopRecording();
        }

        public void OnPreLevelComplete(Evt evt) {
            var player = (PlayerView) evt.Data;
            Everyplay.SetMetadata("Score", player.Score);
            Everyplay.SetMetadata("Level", playerStateStorage.Get().Level);
        }

        private void OnReadyForRecording(bool isReady) {
            // DebugUtils.Log("EveryplayController.OnReadyForRecording()");
            if (!isReady) {
                return;
            }

            Everyplay.SetThumbnailTargetTexture(screenshot);
        }

        private void OnRecordingStarted() {
            coroutineManager.Run(TakeScreenshotLater());
        }

        private IEnumerator TakeScreenshotLater() {
            yield return new WaitForSecondsRealtime(2f);
            Everyplay.TakeThumbnail();
        }
    }
}
