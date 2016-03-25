using UnityEngine;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.LevelModule.Model;
using Catzilla.LevelObjectModule.Model;

namespace Catzilla.LevelObjectModule.View {
    public abstract class BonusView: MonoBehaviour, IPoolable {
        [Inject]
        protected EventBus EventBus;

        public abstract void Accept(BonusVisitor visitor);

        void IPoolable.Reset() {
            EventBus.Fire((int) Events.BonusDestroy, new Evt(this));
        }
    }
}
