using Catzilla.CommonModule.Util;
using Catzilla.LevelObjectModule.View;
using Catzilla.PlayerModule.View;

namespace Catzilla.PlayerModule.Controller {
    public class PlayerHealthController {
        private PlayerView player;
        private PlayerHealthView health;

        public void OnPlayerConstruct(Evt evt) {
            player = (PlayerView) evt.Source;
            health = player.HUD.Health;
            health.MaxValue = player.Health;
        }

        public void OnChange(Evt evt) {
            health.Value = player.Health;
        }
    }
}
