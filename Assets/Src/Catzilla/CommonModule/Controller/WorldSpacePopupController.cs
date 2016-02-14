using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;

namespace Catzilla.CommonModule.Controller {
    public class WorldSpacePopupController {
        [Inject("UILowPrioAudioChannel")]
        public int UILowPrioAudioChannel {get; set;}

        [Inject]
        public AudioManager AudioManager {get; set;}

        public void OnShow(Evt evt) {
            var popup = (WorldSpacePopupView) evt.Source;

            if (popup.ShowSound != null) {
                AudioManager.Play(popup.ShowSound, popup.AudioSource,
                    UILowPrioAudioChannel);
            }
        }
    }
}
