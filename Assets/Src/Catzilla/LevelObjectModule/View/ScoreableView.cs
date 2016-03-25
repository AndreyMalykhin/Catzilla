using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;

namespace Catzilla.LevelObjectModule.View {
    public class ScoreableView: MonoBehaviour {
        public int MinScore;
        public int MaxScore;
        public Collider Collider;
    }
}
