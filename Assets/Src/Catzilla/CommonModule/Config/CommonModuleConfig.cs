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
            injectionBinder.Bind<LanguageManager>()
                .ToValue(LanguageManager.Instance)
                .ToInject(false)
                .CrossContext();
        }

        void IModuleConfig.PostBindings(IInjectionBinder injectionBinder) {
            InitTranslator(injectionBinder);
        }

        private void InitTranslator(IInjectionBinder injectionBinder) {
            var translator = injectionBinder.GetInstance<LanguageManager>();
            GameObject.DontDestroyOnLoad(translator);
            SmartCultureInfo language =
                translator.GetDeviceCultureIfSupported();

            if (language != null) {
                translator.ChangeLanguage(language);
            }
        }
    }
}
