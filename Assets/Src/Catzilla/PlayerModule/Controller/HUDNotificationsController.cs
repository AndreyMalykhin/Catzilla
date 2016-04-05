using UnityEngine;
using System.Text;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;
using Catzilla.LevelObjectModule.View;
using Catzilla.PlayerModule.View;

namespace Catzilla.PlayerModule.Controller {
    public class HUDNotificationsController {
        [Inject]
        private Translator translator;

        private HUDNotificationsView hudNotifications;
        private readonly StringBuilder strBuilder = new StringBuilder(32);

        public void OnConstruct(Evt evt) {
            hudNotifications = (HUDNotificationsView) evt.Source;
        }

        public void OnSmashStreak(Evt evt) {
            Profiler.BeginSample("HUDNotificationsController.OnSmashStreak()");
            var showable = hudNotifications.GetComponent<ShowableView>();
            showable.Hide();
            var smashStreak = (PlayerView.SmashStreak) evt.Data;
            hudNotifications.Msg.text = strBuilder.Append("x")
                .Append(smashStreak.Length)
                .Append(' ')
                .Append(translator.Translate("Player.SmashStreak"))
                .Append("\n+")
                .Append(smashStreak.ExtraScore)
                .ToString();
            strBuilder.Length = 0;
            showable.Show();
            Profiler.EndSample();
        }
    }
}
