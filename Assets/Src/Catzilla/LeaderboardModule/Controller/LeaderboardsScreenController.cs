using Catzilla.CommonModule.Util;
using Catzilla.LeaderboardModule.View;

namespace Catzilla.LeaderboardModule.Controller {
    public class LeaderboardsScreenController {
        [Inject]
        public LeaderboardsScreenView LeaderboardsScreen {get; set;}

        public void OnBackBtnClick(Evt evt) {
            LeaderboardsScreen.Hide();
        }
    }
}
