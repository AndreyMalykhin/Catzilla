using System.Text;
using Zenject;
using Catzilla.CommonModule.View;
using Catzilla.CommonModule.Util;
using Catzilla.LevelObjectModule.View;

namespace Catzilla.PlayerModule.Model {
    public class PlayerManager {
        [Inject]
        private WorldSpacePopupManager popupManager;

        private readonly StringBuilder strBuilder = new StringBuilder(8);

        public void AddScore(PlayerView player, ScoreableView scoreable) {
            player.Score += scoreable.Score;

            if (player.IsScoreFreezed) {
                return;
            }

            WorldSpacePopupView popup = popupManager.Get();
            popup.PlaceAbove(scoreable.Collider.bounds);
            popup.LookAtTarget = player.Camera;
            popup.Msg.text =
                strBuilder.Append('+').Append(scoreable.Score).ToString();
            strBuilder.Length = 0;
            popupManager.Show(popup);
        }
    }
}
