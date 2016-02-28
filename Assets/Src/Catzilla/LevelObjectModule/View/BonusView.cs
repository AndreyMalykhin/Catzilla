using UnityEngine;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.LevelModule.Model;

namespace Catzilla.LevelObjectModule.View {
    public abstract class BonusView: MonoBehaviour, IPoolable {
        public enum Event {Destroy}

        [Inject]
        protected EventBus EventBus;

        public abstract bool IsNeeded(
            PlayerView player, LevelSettings levelSettings);

        public abstract bool CanGive(
            PlayerView player, LevelSettings levelSettings);

        void IPoolable.Reset() {
            EventBus.Fire(Event.Destroy, new Evt(this));
        }
    }
}
