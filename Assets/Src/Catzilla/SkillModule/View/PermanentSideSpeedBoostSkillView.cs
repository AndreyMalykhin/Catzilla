using UnityEngine;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.LevelObjectModule.Model;
using Catzilla.LevelObjectModule.View;
using Catzilla.SkillModule.Model;

namespace Catzilla.SkillModule.View {
    public class PermanentSideSpeedBoostSkillView: MonoBehaviour {
        public void Init(PlayerView player, float factor) {
            // DebugUtils.Log("PermanentSideSpeedBoostSkillView.Init()");
            player.SideSpeed += player.BaseSideSpeed * factor;
        }
    }
}
