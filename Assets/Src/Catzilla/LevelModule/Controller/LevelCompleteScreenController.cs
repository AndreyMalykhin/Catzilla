using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Model;
using Catzilla.CommonModule.Util;
using Catzilla.LevelModule.View;

namespace Catzilla.LevelModule.Controller {
    public class LevelCompleteScreenController {
        [Inject]
        public AudioManager AudioManager {get; set;}

        [Inject("UIAudioChannel")]
        public int UIAudioChannel {get; set;}

        public void OnShow(Evt evt) {
            var screen = (LevelCompleteScreenView) evt.Source;

            if (screen.ShowSound != null) {
                AudioManager.Play(
                    screen.ShowSound, screen.AudioSource, UIAudioChannel);
            }
        }
    }
}
