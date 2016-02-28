using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;

namespace Catzilla.LevelObjectModule.View {
    public class ScoreableView: MonoBehaviour {
        public int Score = 1;
        public Collider Collider;
    }
}
