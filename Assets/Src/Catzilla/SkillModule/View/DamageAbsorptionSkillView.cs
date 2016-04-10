using UnityEngine;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.LevelObjectModule.Model;
using Catzilla.LevelObjectModule.View;

namespace Catzilla.SkillModule.View {
    public class DamageAbsorptionSkillView: MonoBehaviour {
        public float Chance;
        public float Factor;

        [Inject]
        private EventBus eventBus;

        [PostInject]
        public void OnConstruct() {
            eventBus.Fire(
                (int) Events.DamageAbsorptionSkillConstruct, new Evt(this));
        }

        public Attack OnAttackFilter(
            Attack attack, DamagingView source = null) {
            // DebugUtils.Log("DamageAbsorptionSkillView.OnAttackFilter()");
            if (source != null && UnityEngine.Random.value <= Chance) {
                int damage = attack.Damage;
                attack.Damage = damage - Mathf.RoundToInt(damage * Factor);
                attack.Status = AttackStatus.Absorb;
                return attack;
            }

            return attack;
        }
    }
}
