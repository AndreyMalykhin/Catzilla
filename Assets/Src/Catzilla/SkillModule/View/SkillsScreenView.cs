using UnityEngine;
using Zenject;
using System.Collections.Generic;

namespace Catzilla.SkillModule.View {
    public class SkillsScreenView: MonoBehaviour {
        public List<SkillListItemView.Item> Items {
            set {
                for (int i = 0; i < items.Count; ++i) {
                    Destroy(items[i].gameObject);
                }

                items.Clear();

                for (int i = 0; i < value.Count; ++i) {
                    items.Add(CreateItem(value[i]));
                }
            }
        }

        [Inject]
        private IInstantiator instantiator;

        [SerializeField]
        private Transform itemList;

        [SerializeField]
        private SkillListItemView itemProto;

        private List<SkillListItemView> items = new List<SkillListItemView>(8);

        private SkillListItemView CreateItem(SkillListItemView.Item itemState) {
            var itemView = instantiator.InstantiatePrefab(itemProto.gameObject)
                .GetComponent<SkillListItemView>();
            bool isWorldPositionStays = false;
            itemView.transform.SetParent(itemList, isWorldPositionStays);
            itemView.SetItem(itemState);
            return itemView;
        }
    }
}
