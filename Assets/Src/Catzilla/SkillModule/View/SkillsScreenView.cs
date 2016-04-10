using UnityEngine;
using UnityEngine.UI;
using Zenject;
using System.Collections.Generic;
using Catzilla.CommonModule.View;
using Catzilla.CommonModule.Util;

namespace Catzilla.SkillModule.View {
    public class SkillsScreenView: MonoBehaviour {
        public List<SkillListItemView.Item> Items {
            set {
                for (int i = items.Count; i < value.Count; ++i) {
                    items.Add(CreateItem());
                }

                for (int i = items.Count - 1; i >= value.Count; --i) {
                    DestroyItem(items[i]);
                    items.RemoveAt(i);
                }

                for (int i = 0; i < value.Count; ++i) {
                    items[i].SetItem(value[i]);
                }
            }
        }

        public Text AvailableSkillPoints {get {return availableSkillPoints;}}
        public Button CloseBtn {get {return closeBtn;}}

        [Inject]
        private PoolStorageView poolStorage;

        [SerializeField]
        private Transform itemList;

        [SerializeField]
        private SkillListItemView itemProto;

        [SerializeField]
        private Text availableSkillPoints;

        [SerializeField]
        private Button closeBtn;

        private readonly List<SkillListItemView> items =
            new List<SkillListItemView>(8);

        private SkillListItemView CreateItem() {
            // DebugUtils.Log("SkillsScreenView.CreateItem()");
            int poolId = itemProto.GetComponent<PoolableView>().PoolId;
            var itemView =
                poolStorage.Take(poolId).GetComponent<SkillListItemView>();
            bool isWorldPositionStays = false;
            itemView.transform.SetParent(itemList, isWorldPositionStays);
            itemView.transform.SetAsLastSibling();
            return itemView;
        }

        private void DestroyItem(SkillListItemView item) {
            poolStorage.Return(item.GetComponent<PoolableView>());
        }
    }
}
