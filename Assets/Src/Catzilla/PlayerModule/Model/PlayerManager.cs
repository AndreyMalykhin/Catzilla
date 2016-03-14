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
            int score = UnityEngine.Random.Range(
                scoreable.MinScore, scoreable.MaxScore + 1);
            player.Score += score;

            if (player.IsScoreFreezed) {
                return;
            }

            WorldSpacePopupView popup = popupManager.Get();
            popup.PlaceAbove(scoreable.Collider.bounds);
            popup.LookAtTarget = player.Camera;
            popup.Msg.text =
                strBuilder.Append('+').Append(score).ToString();
            strBuilder.Length = 0;
            popupManager.Show(popup);
        }
    }
}
