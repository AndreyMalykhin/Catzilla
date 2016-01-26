using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SmartLocalization;

namespace Catzilla.CommonModule.View {
    public class TranslatorView: strange.extensions.mediation.impl.View {
        [Inject]
        public LanguageManager Translator {get; set;}

        [System.Serializable]
        private struct Item {
            public string Key;
            public Text Text;
        }

        [SerializeField]
        private Item[] items;

        [PostConstruct]
        public void OnConstruct() {
            for (int i = 0; i < items.Length; ++i) {
                Item item = items[i];
                item.Text.text = Translator.GetTextValue(item.Key);
            }
        }
    }
}
