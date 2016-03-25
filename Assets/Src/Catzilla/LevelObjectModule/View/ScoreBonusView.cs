using UnityEngine;
using Catzilla.CommonModule.Util;
using Catzilla.LevelObjectModule.Model;

namespace Catzilla.LevelObjectModule.View {
    public class ScoreBonusView: BonusView {
        public ScoreableView Scoreable {get {return scoreable;}}

        [SerializeField]
        private ScoreableView scoreable;

        public override void Accept(BonusVisitor visitor) {
            visitor.Visit(this);
        }
    }
}
