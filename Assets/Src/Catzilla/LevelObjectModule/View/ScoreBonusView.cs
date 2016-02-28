using UnityEngine;
using Catzilla.CommonModule.Util;
using Catzilla.LevelModule.Model;

namespace Catzilla.LevelObjectModule.View {
    public class ScoreBonusView: BonusView {
        [SerializeField]
        private ScoreableView scoreable;

        public override bool IsNeeded(
            PlayerView player, LevelSettings levelSettings) {
            return player.Score > 0.125f * levelSettings.CompletionScore;
        }

        public override bool CanGive(
            PlayerView player, LevelSettings levelSettings) {
            // DebugUtils.Log("ScoreBonusView.CanGive(); taken={0}; extra={1}",
            //     player.ScoreBonusesTaken, levelSettings.ExtraScore);
            int availableBonuses = levelSettings.ExtraScore / scoreable.Score;
            return player.ScoreBonusesTaken < availableBonuses;
        }
    }
}
