using Catzilla.CommonModule.Util;
using Catzilla.LevelObjectModule.View;
using Catzilla.PlayerModule.View;

namespace Catzilla.PlayerModule.Controller {
    public class HUDHealthController {
        private PlayerView player;
        private HUDHealthView health;

        public void OnPlayerConstruct(Evt evt) {
            player = (PlayerView) evt.Source;
            health = player.HUD.Health;
            health.MaxValue = player.MaxHealth;
            health.Value = player.Health;
        }

        public void OnChange(Evt evt) {
            health.Value = player.Health;
        }
    }
}
