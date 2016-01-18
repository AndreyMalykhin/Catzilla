using UnityEngine;
using System.Collections;
using strange.extensions.injector.api;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using SmartLocalization;
using Catzilla.CommonModule.View;

namespace Catzilla.CommonModule.Config {
    public class CommonModuleConfig: IModuleConfig {
        void IModuleConfig.InitBindings(IInjectionBinder injectionBinder) {
            InitTranslator(LanguageManager.Instance);
            injectionBinder.Bind<LanguageManager>()
                .ToValue(LanguageManager.Instance)
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
