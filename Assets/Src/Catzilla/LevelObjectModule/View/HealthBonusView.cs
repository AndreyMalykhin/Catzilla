using UnityEngine;
using Catzilla.LevelObjectModule.Model;

namespace Catzilla.LevelObjectModule.View {
    public class HealthBonusView: BonusView {
        public override void Accept(BonusVisitor visitor) {
            visitor.Visit(this);
        }
    }
}
