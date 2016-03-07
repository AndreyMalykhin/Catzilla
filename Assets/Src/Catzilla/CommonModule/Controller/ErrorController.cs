using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;

namespace Catzilla.CommonModule.Controller {
    public class ErrorController {
        [Inject("ErrorMsg")]
        private string errorMsg;

        [Inject("ErrorPopupType")]
        private int errorPopupType;

        [Inject]
        private ScreenSpacePopupManagerView popupManager;

        [Inject]
        private Translator translator;

        public void OnServerResponse(Evt evt) {
            bool isSuccess = (bool) evt.Data;

            if (isSuccess) {
                return;
            }

            ScreenSpacePopupView popup = popupManager.Get(errorPopupType);
            popup.Msg.text = translator.Translate(errorMsg);
            popupManager.Show(popup);
        }
    }
}
