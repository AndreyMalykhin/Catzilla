using UnityEngine;
using Catzilla.LevelObjectModule.Model;

namespace Catzilla.LevelObjectModule.View {
    public class ResurrectionBonusView: BonusView {
        public Collider Collider {get {return collider;}}

        [SerializeField]
        new private Collider collider;

        public override void Accept(BonusVisitor visitor) {
            visitor.Visit(this);
        }
    }
}
