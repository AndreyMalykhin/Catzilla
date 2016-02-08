using UnityEngine;
using System.Collections;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.Model;
using Catzilla.CommonModule.Util;
using Catzilla.LevelModule.View;

namespace Catzilla.LevelModule.Controller {
    public class LevelCompleteScreenController {
        [Inject]
        public Game Game {get; set;}

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

        public void OnHide(Evt evt) {
            Game.LoadLevel();
        }
    }
}
