using UnityEngine;
using System.Collections;
using strange.extensions.injector.api;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using SmartLocalization;
using Catzilla.CommonModule.View;
using Catzilla.CommonModule.Model;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.Controller;

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
            injectionBinder.Bind<GameController>()
                .To<GameController>()
                .ToSingleton()
                .CrossContext();
            injectionBinder.Bind<Ad>()
                .To<Ad>()
                .ToSingleton()
                .CrossContext();
            injectionBinder.Bind<DisposableController>()
                .To<DisposableController>()
                .ToSingleton()
                .CrossContext();
            injectionBinder.Bind<PoolableController>()
                .To<PoolableController>()
                .ToSingleton()
                .CrossContext();
            var ui = GameObject.FindWithTag("UI").GetComponent<UIView>();
            injectionBinder.Bind<UIView>()
                .ToValue(ui)
                .ToInject(false)
                .CrossContext();
            var poolStorage = GameObject.FindWithTag("PoolStorage")
                .GetComponent<PoolStorageView>();
            injectionBinder.Bind<PoolStorageView>()
                .ToValue(poolStorage)
                .ToInject(false)
                .CrossContext();
        }

        void IModuleConfig.PostBindings(IInjectionBinder injectionBinder) {
            var eventBus = injectionBinder.GetInstance<IEventDispatcher>(
                ContextKeys.CROSS_CONTEXT_DISPATCHER);
            var disposableController =
                injectionBinder.GetInstance<DisposableController>();
            eventBus.AddListener(DisposableView.Event.TriggerExit,
                disposableController.OnTriggerExit);

            var gameController =
                injectionBinder.GetInstance<GameController>();
            eventBus.AddListener(
                Game.Event.LevelLoad, gameController.OnLevelLoad);
        }

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
