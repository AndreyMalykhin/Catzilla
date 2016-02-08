using UnityEngine;
using System.Collections;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.View;
using Catzilla.CommonModule.Util;

namespace Catzilla.CommonModule.Controller {
    public class BtnController {
        [Inject]
        public AudioManager AudioManager {get; set;}

        [Inject("UIAudioChannel")]
        public int UIAudioChannel {get; set;}

        public void OnClick(Evt evt) {
            var btn = (BtnView) evt.Source;

            if (btn.ClickSound != null) {
                AudioManager.Play(
                    btn.ClickSound, btn.AudioSource, UIAudioChannel);
            }
        }
    }
}
