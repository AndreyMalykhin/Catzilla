using UnityEngine;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.LevelObjectModule.Model;
using Catzilla.LevelObjectModule.View;
using Catzilla.SkillModule.Model;

namespace Catzilla.SkillModule.View {
    public class CriticalSmashSkillView: MonoBehaviour {
        public float Duration {
            get {return temporary.Duration;}
            set {temporary.Duration = value;}
        }

        public float Chance;
        public float Factor;

        [Inject]
        private EventBus eventBus;

        private PlayerView player;
        private TemporarySkill temporary;

        [PostInject]
        public void OnConstruct() {
            temporary = new TemporarySkill(this, Activate, "Deactivate");
            temporary.Chance = 1f;
            eventBus.Fire(
                (int) Events.CriticalSmashSkillConstruct, new Evt(this));
        }

        public void Init(PlayerView player) {
            this.player = player;
        }

        public void Trigger() {
            temporary.Trigger();
        }

        private void Activate() {
            player.CriticalScoreChance += Chance;
            player.CriticalScoreFactor += Factor;
        }

        private void Deactivate() {
            player.CriticalScoreChance -= Chance;
            player.CriticalScoreFactor -= Factor;
        }
    }
}
