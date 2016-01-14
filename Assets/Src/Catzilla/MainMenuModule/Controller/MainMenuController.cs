using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SmartLocalization;
using Catzilla.MainMenuModule.View;

namespace Catzilla.MainMenuModule.Controller {
    public class MainMenuController {
        [Inject]
        public LanguageManager Translator {get; set;}

        [Inject]
        public MainMenuView MainMenu {get; set;}

        [PostConstruct]
        public void OnReady() {
            MainMenu.StartText.text = Translator.GetTextValue("MainMenu.Start");
            MainMenu.ExitText.text = Translator.GetTextValue("MainMenu.Exit");
        }
    }
}
