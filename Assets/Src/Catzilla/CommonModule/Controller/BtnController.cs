using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.View;
using Catzilla.CommonModule.Util;

namespace Catzilla.CommonModule.Controller {
    public class BtnController {
        [Inject]
        private AudioManager audioManager;

        [Inject("UIHighPrioAudioChannel")]
        private int uiHighPrioAudioChannel;

        [Inject]
        private UIView ui;

        public void OnClick(Evt evt) {
            var btn = (BtnView) evt.Source;

            if (btn.ClickSound != null) {
                AudioSource audioSource = btn.AudioSource == null ?
                    ui.AudioSource : btn.AudioSource;
                audioManager.Play(
                    btn.ClickSound, audioSource, uiHighPrioAudioChannel);
            }
        }
    }
}
