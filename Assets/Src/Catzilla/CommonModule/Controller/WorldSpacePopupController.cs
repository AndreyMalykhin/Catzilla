using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;

namespace Catzilla.CommonModule.Controller {
    public class WorldSpacePopupController {
        [Inject("UIAudioChannel")]
        public int UIAudioChannel {get; set;}

        [Inject]
        public AudioManager AudioManager {get; set;}

        public void OnShow(Evt evt) {
            var popup = (WorldSpacePopupView) evt.Source;

            if (popup.ShowSound != null) {
                AudioManager.Play(popup.ShowSound, popup.AudioSource,
                    UIAudioChannel);
            }
        }
    }
}
