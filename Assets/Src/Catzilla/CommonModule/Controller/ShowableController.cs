using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Model;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;

namespace Catzilla.CommonModule.Controller {
    public class ShowableController {
        [Inject]
        private AudioManager audioManager;

        [Inject("UIHighPrioAudioChannel")]
        private int uiHighPrioAudioChannel;

        public void OnShow(Evt evt) {
            var showable = (ShowableView) evt.Source;

            if (showable.ShowSound != null) {
                audioManager.Play(showable.ShowSound, showable.AudioSource,
                    uiHighPrioAudioChannel);
            }
        }

        public void OnPreShow(Evt evt) {
            var showable = (ShowableView) evt.Source;

            if (showable.PreShowSound != null) {
                audioManager.Play(showable.PreShowSound, showable.AudioSource,
                    uiHighPrioAudioChannel);
            }
        }
    }
}
