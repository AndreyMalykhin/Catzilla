using UnityEngine;
using System.Collections;
using strange.extensions.injector.api;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using SmartLocalization;
using Catzilla.CommonModule.View;
using Catzilla.CommonModule.Model;

namespace Catzilla.CommonModule.Config {
    public class CommonModuleConfig: IModuleConfig {
        void IModuleConfig.InitBindings(IInjectionBinder injectionBinder) {
            InitTranslator(LanguageManager.Instance);
            injectionBinder.Bind<LanguageManager>()
                .ToValue(LanguageManager.Instance)
                .ToInject(false)
                .CrossContext();
            injectionBinder.Bind<Game>()
                .To<Game>()
                .ToSingleton()
                .CrossContext();
            var ui = GameObject.FindWithTag("UI").GetComponent<UIView>();
            injectionBinder.Bind<UIView>()
                .ToValue(ui)
                .ToInject(false)
                .CrossContext();
        }

        void IModuleConfig.PostBindings(IInjectionBinder injectionBinder) {}

        private void InitTranslator(LanguageManager translator) {
            GameObject.DontDestroyOnLoad(translator);
            SmartCultureInfo language =
                translator.GetDeviceCultureIfSupported();

            if (language != null) {
                translator.ChangeLanguage(language);
            }
        }
    }
}
