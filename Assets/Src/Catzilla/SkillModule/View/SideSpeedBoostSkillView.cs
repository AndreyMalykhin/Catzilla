using UnityEngine;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.LevelObjectModule.Model;
using Catzilla.LevelObjectModule.View;
using Catzilla.SkillModule.Model;

namespace Catzilla.SkillModule.View {
    public class SideSpeedBoostSkillView: MonoBehaviour {
        public float Chance {
            get {return temporary.Chance;}
            set {temporary.Chance = value;}
        }

        public float Duration {
            get {return temporary.Duration;}
            set {temporary.Duration = value;}
        }

        public float Factor;

        [Inject]
        private EventBus eventBus;

        private PlayerView player;
        private TemporarySkill temporary;

        [PostInject]
        public void OnConstruct() {
            temporary = new TemporarySkill(this, Activate, "Deactivate");
            eventBus.Fire(
                (int) Events.SideSpeedBoostSkillConstruct, new Evt(this));
        }

        public void Init(PlayerView player) {
            this.player = player;
        }

        public void Trigger() {
            temporary.Trigger();
        }

        private void Activate() {
            // DebugUtils.Log("SideSpeedBoostSkillView.Activate()");
            player.SideSpeed += GetSpeedDelta();
        }

        private void Deactivate() {
            // DebugUtils.Log("SideSpeedBoostSkillView.Deactivate()");
            player.SideSpeed -= GetSpeedDelta();
        }

        private float GetSpeedDelta() {
            return player.BaseSideSpeed * Factor;
        }
    }
}
