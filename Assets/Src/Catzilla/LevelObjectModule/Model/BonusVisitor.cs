using Catzilla.LevelObjectModule.View;

namespace Catzilla.LevelObjectModule.Model {
    public interface BonusVisitor {
        void Visit(ScoreBonusView scoreBonus);
        void Visit(HealthBonusView healthBonus);
        void Visit(ResurrectionBonusView resurrectionBonus);
        void Visit(RewardBonusView rewardBonus);
    }
}
