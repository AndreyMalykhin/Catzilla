using UnityEngine;
using UnityEngine.UI;
using Zenject;
using System.Collections.Generic;
using Catzilla.CommonModule.Util;

namespace Catzilla.SkillModule.View {
    public class SkillListItemView: MonoBehaviour {
        public struct Item {
            public Sprite Img;
            public string Name;
            public string CurrentLevel;
            public string CurrentLevelDescription;
            public int NextLevelId;
            public string NextLevelDescription;
            public bool IsDisabled;
            public string Learn;
        }

        [Inject]
        private EventBus eventBus;

        [SerializeField]
        private Image img;

        [SerializeField]
        new private Text name;

        [SerializeField]
        private Text currentLevelDescription;

        [SerializeField]
        private Text nextLevelDescription;

        [SerializeField]
        private Text currentLevel;

        [SerializeField]
        private Button learnBtn;

        [SerializeField]
        private Text learnBtnText;

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
            currentLevelDescription.gameObject.SetActive(
                item.CurrentLevelDescription != "");
            currentLevelDescription.text = item.CurrentLevelDescription;
            nextLevelDescription.gameObject.SetActive(
                item.NextLevelDescription != "");
            nextLevelDescription.text = item.NextLevelDescription;
            learnBtn.gameObject.SetActive(item.NextLevelDescription != "");
            learnBtn.interactable = !item.IsDisabled;
            learnBtnText.text = item.Learn;
            currentLevel.text = item.CurrentLevel;
            img.sprite = item.Img;
            name.text = item.Name;
        }

        private void OnLearnBtnClick() {
            eventBus.Fire((int) Events.SkillListItemLearnBtnClick,
                new Evt(this, item.NextLevelId));
        }
    }
}
