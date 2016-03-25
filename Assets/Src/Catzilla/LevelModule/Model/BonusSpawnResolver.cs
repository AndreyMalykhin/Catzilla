using Zenject;
using Catzilla.PlayerModule.Model;
using Catzilla.LevelObjectModule.Model;
using Catzilla.LevelObjectModule.View;

namespace Catzilla.LevelModule.Model {
    public class BonusSpawnResolver: BonusVisitor {
        public int ActiveBonusObjects;

        [Inject]
        private LevelSettingsStorage levelSettingsStorage;

        [Inject]
        private PlayerStateStorage playerStateStorage;

        private PlayerView player;
        private bool result;

        public bool IsTimeToSpawn(BonusView bonus, PlayerView player) {
            if (ActiveBonusObjects > 0) {
                return false;
            }

            this.player = player;
            bonus.Accept(this);
            return result;
        }

        void BonusVisitor.Visit(ScoreBonusView scoreBonus) {
            LevelSettings levelSettings = GetLevelSettings();
            ScoreableView scoreable = scoreBonus.Scoreable;
            int availableBonuses = levelSettings.ExtraScore /
                ((scoreable.MinScore + scoreable.MaxScore) / 2);
            result = player.Score >= 0.125f * levelSettings.CompletionScore
                && player.ScoreBonusesTaken < availableBonuses;
        }

        void BonusVisitor.Visit(HealthBonusView healthBonus) {
            result = player.Health < player.MaxHealth;
        }

        void BonusVisitor.Visit(ResurrectionBonusView resurrectionBonus) {
            LevelSettings levelSettings = GetLevelSettings();
            result = player.Score >= 0.25f * levelSettings.CompletionScore
                && player.Score <= 0.75f * levelSettings.CompletionScore
                && playerStateStorage.Get().AvailableResurrectionsCount == 0;
        }

        void BonusVisitor.Visit(RewardBonusView rewardBonus) {
            LevelSettings levelSettings = GetLevelSettings();
            result = player.Score >= 0.25f * levelSettings.CompletionScore
                && playerStateStorage.Get().AvailableRewardsCount == 0;
        }

        private LevelSettings GetLevelSettings() {
            return levelSettingsStorage.Get(playerStateStorage.Get().Level);
        }
    }
}
