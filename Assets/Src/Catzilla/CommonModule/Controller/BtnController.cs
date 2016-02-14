using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.View;
using Catzilla.CommonModule.Util;

namespace Catzilla.CommonModule.Controller {
    public class BtnController {
        [Inject]
        public AudioManager AudioManager {get; set;}

        [Inject("UIHighPrioAudioChannel")]
        public int UIHighPrioAudioChannel {get; set;}

        public void OnClick(Evt evt) {
            var btn = (BtnView) evt.Source;

            if (btn.ClickSound != null) {
                AudioManager.Play(
                    btn.ClickSound, btn.AudioSource, UIHighPrioAudioChannel);
            }
        }
    }
}
