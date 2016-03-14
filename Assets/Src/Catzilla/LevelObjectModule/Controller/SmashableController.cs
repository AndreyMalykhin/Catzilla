using UnityEngine;
using System.Collections;
using System.Text;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;
using Catzilla.PlayerModule.Model;
using Catzilla.LevelObjectModule.View;

namespace Catzilla.LevelObjectModule.Controller {
    public class SmashableController {
        [Inject("EffectsHighPrioAudioChannel")]
        private int effectsAudioChannel;

        [Inject]
        private AudioManager audioManager;

        public void OnSmash(Evt evt) {
            var smashed = (SmashedView) evt.Data;

            if (smashed.SmashSound != null) {
                var pitch = UnityEngine.Random.Range(0.95f, 1.05f);
                audioManager.Play(smashed.SmashSound, smashed.AudioSource,
                    effectsAudioChannel, pitch);
            }
        }
    }
}
