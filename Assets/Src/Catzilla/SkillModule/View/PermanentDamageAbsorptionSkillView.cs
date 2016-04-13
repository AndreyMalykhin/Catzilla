using UnityEngine;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.LevelObjectModule.Model;
using Catzilla.LevelObjectModule.View;
using Catzilla.SkillModule.Model;

namespace Catzilla.SkillModule.View {
    public class PermanentDamageAbsorptionSkillView: MonoBehaviour {
        public void Init(PlayerView player, float chance, float factor) {
            // DebugUtils.Log("PermanentDamageAbsorptionSkillView.Init()");
            player.DamageAbsorbChance += chance;
            player.DamageAbsorbFactor += factor;
        }
    }
}
