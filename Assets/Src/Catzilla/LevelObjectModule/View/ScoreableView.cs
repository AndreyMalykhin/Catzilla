using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;

namespace Catzilla.LevelObjectModule.View {
    public class ScoreableView: MonoBehaviour {
        public int MinScore = 1;
        public int MaxScore = 2;
        public Collider Collider;
    }
}
