using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Catzilla.CommonModule.Model;

namespace Catzilla.LeaderboardModule.View {
    public class ScoreLeaderboardView
        : strange.extensions.mediation.impl.View {
        public List<ScoreLeaderboardItem> Items {
            set {
                Clear();

                for (int i = 0; i < value.Count; ++i) {
                    AddItem(value[i]);
                }
            }
        }

        [SerializeField]
        private ScoreLeaderboardItemView itemProto;

        private readonly List<ScoreLeaderboardItemView> itemViews =
            new List<ScoreLeaderboardItemView>(10);

        private void Clear() {
            for (int i = 0; i < itemViews.Count; ++i) {
                Destroy(itemViews[i].gameObject);
            }

            itemViews.Clear();
        }

        private void AddItem(ScoreLeaderboardItem item) {
            var itemView = (ScoreLeaderboardItemView) Instantiate(itemProto);
            bool isWorldPositionStays = false;
            itemView.transform.SetParent(transform, isWorldPositionStays);
            itemView.Item = item;
            itemViews.Add(itemView);
        }
    }
}
