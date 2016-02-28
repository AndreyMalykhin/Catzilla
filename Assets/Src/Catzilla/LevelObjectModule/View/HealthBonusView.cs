using UnityEngine;
using Catzilla.LevelModule.Model;

namespace Catzilla.LevelObjectModule.View {
    public class HealthBonusView: BonusView {
        public override bool IsNeeded(
            PlayerView player, LevelSettings levelSettings) {
            return player.Health < player.MaxHealth;
        }

        public override bool CanGive(
            PlayerView player, LevelSettings levelSettings) {
            return true;
        }
    }
}
