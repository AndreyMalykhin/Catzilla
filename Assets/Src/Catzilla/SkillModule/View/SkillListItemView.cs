using UnityEngine;
using UnityEngine.UI;
using Zenject;
using System.Collections.Generic;
using Catzilla.CommonModule.Util;

namespace Catzilla.SkillModule.View {
    public class SkillListItemView: MonoBehaviour {
        public struct Item {
            public readonly int BaseId;
            public readonly Texture2D Img;
            public readonly string Name;
            public readonly int CurrentLevel;
            public readonly string CurrentLevelDescription;
            public readonly int MaxLevel;
            public readonly int NextLevelId;
            public readonly string NextLevelDescription;
            public readonly bool IsAvailable;

            public Item(
                int baseId,
                Texture2D img,
                string name,
                int currentLevel,
                string currentLevelDescription,
                int maxLevel,
                int nextLevelId,
                string nextLevelDescription,
                bool isAvailable) {
                BaseId = baseId;
                Img = img;
                Name = name;
                CurrentLevel = currentLevel;
                CurrentLevelDescription = currentLevelDescription;
                MaxLevel = maxLevel;
                NextLevelId = nextLevelId;
                NextLevelDescription = nextLevelDescription;
                IsAvailable = isAvailable;
            }
        }

        [Inject]
        private EventBus eventBus;

        [SerializeField]
        private RawImage img;

        [SerializeField]
        new private Text name;

        [SerializeField]
        private Text currentLevelDescription;

        [SerializeField]
        private Text nextLevelDescription;

        [SerializeField]
        private Text level;

        [SerializeField]
        private Button learnBtn;

        private Item item;

        [PostInject]
        public void OnConstruct() {
            learnBtn.onClick.AddListener(OnLearnBtnClick);
        }

        public void SetItem(Item item) {
            this.item = item;
            Render();
        }

        private void Render() {
            img.texture = item.Img;
            name.text = item.Name;
            currentLevelDescription.text = item.CurrentLevelDescription;
            nextLevelDescription.text = item.NextLevelDescription;
            level.text = string.Format("{0} / {1}",
                Mathf.Max(item.CurrentLevel, 0), item.MaxLevel);
            currentLevelDescription.gameObject.SetActive(
                item.CurrentLevel != -1);
            learnBtn.gameObject.SetActive(item.CurrentLevel != item.MaxLevel);
            learnBtn.interactable = item.IsAvailable;
        }

        private void OnLearnBtnClick() {
            eventBus.Fire((int) Events.SkillListItemLearnBtnClick,
                new Evt(this, item.NextLevelId));
        }
    }
}
