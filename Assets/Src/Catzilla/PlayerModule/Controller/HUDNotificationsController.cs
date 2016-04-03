using UnityEngine;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;
using Catzilla.LevelObjectModule.View;
using Catzilla.PlayerModule.View;

namespace Catzilla.PlayerModule.Controller {
    public class HUDNotificationsController {
        [Inject("SmashStreakPopupType")]
        private int smashStreakPopupType;

        [Inject]
        private Translator translator;

        private HUDNotificationsView hudNotifications;

        public void OnConstruct(Evt evt) {
            hudNotifications = (HUDNotificationsView) evt.Source;
        }

        public void OnSmashStreak(Evt evt) {
            var popupManager = hudNotifications.PopupManager;
            var popup = (ScreenSpacePopupView) popupManager.Get(
                smashStreakPopupType);
            var smashStreak = (PlayerView.SmashStreak) evt.Data;
            Profiler.BeginSample("HUDNotificationsController.OnSmashStreak()");
            popup.Msg.text = translator.Translate("Player.SmashStreak",
                smashStreak.Length, smashStreak.ExtraScore);
            popupManager.Show(popup);
            Profiler.EndSample();
        }
    }
}
