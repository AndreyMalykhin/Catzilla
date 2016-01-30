using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Catzilla.CommonModule.Model;

namespace Catzilla.LeaderboardModule.View {
    public class ScoreLeaderboardItemView
        : strange.extensions.mediation.impl.View {
        public ScoreLeaderboardItem Item {
            set {
                rankText.text = value.Rank.ToString();
                scoreText.text = value.Score.ToString();
                nameText.text = value.Name;
            }
        }

        [SerializeField]
        private Text rankText;

        [SerializeField]
        private Text scoreText;

        [SerializeField]
        private Text nameText;
    }
}
