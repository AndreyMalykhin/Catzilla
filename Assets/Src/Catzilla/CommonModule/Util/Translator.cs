using UnityEngine;
using SmartLocalization;

namespace Catzilla.CommonModule.Util {
    public class Translator {
        private readonly LanguageManager languageManager =
            LanguageManager.Instance;

        public Translator() {
            GameObject.DontDestroyOnLoad(languageManager);
            // languageManager.ChangeLanguage("ru");
            SmartCultureInfo language =
                languageManager.GetDeviceCultureIfSupported();

            if (language != null) {
                languageManager.ChangeLanguage(language);
            }
        }

        public string Translate(string key) {
            return languageManager.GetTextValue(key);
        }

        public string Translate(string key, params object[] args) {
            return string.Format(languageManager.GetTextValue(key), args);
        }
    }
}
