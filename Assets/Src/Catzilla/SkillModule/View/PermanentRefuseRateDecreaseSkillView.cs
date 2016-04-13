using UnityEngine;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.LevelObjectModule.Model;
using Catzilla.LevelObjectModule.View;
using Catzilla.SkillModule.Model;

namespace Catzilla.SkillModule.View {
    public class PermanentRefuseRateDecreaseSkillView: MonoBehaviour {
        public void Init(PlayerView player, float factor) {
            // DebugUtils.Log("PermanentRefuseRateDecreaseSkillView.Init()");
            player.RefuseChance -= player.BaseRefuseChance * factor;
        }
    }
}
