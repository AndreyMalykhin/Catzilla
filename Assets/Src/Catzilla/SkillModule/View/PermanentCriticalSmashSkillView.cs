using UnityEngine;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.LevelObjectModule.Model;
using Catzilla.LevelObjectModule.View;
using Catzilla.SkillModule.Model;

namespace Catzilla.SkillModule.View {
    public class PermanentCriticalSmashSkillView: MonoBehaviour {
        public void Init(PlayerView player, float chance, float factor) {
            // DebugUtils.Log("PermanentCriticalSmashSkillView.Init()");
            player.CriticalScoreChance += chance;
            player.CriticalScoreFactor += factor;
        }
    }
}
