using UnityEngine;
using System.Collections;
using strange.extensions.injector.api;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using SmartLocalization;
using Catzilla.CommonModule.Config;
using Catzilla.LevelObjectModule.View;
using Catzilla.LevelModule.View;
using Catzilla.PlayerModule.Model;
using Catzilla.PlayerModule.View;
using Catzilla.PlayerModule.Controller;

namespace Catzilla.PlayerModule.Config {
    public class PlayerModuleConfig: IModuleConfig {
        void IModuleConfig.InitBindings(IInjectionBinder injectionBinder) {
            injectionBinder.Bind<PlayerStateStorage>()
                .To<PlayerStateStorage>()
                .ToSingleton()
                .CrossContext();
            injectionBinder.Bind<PlayerScoreController>()
                .To<PlayerScoreController>()
                .ToSingleton()
                .CrossContext();
            injectionBinder.Bind<PlayerStateStorageController>()
                .To<PlayerStateStorageController>()
                .ToSingleton()
                .CrossContext();
        }

        void IModuleConfig.PostBindings(IInjectionBinder injectionBinder) {
            var eventBus = injectionBinder.GetInstance<IEventDispatcher>(
                ContextKeys.CROSS_CONTEXT_DISPATCHER);
            var playerScoreController =
                injectionBinder.GetInstance<PlayerScoreController>();
            eventBus.AddListener(
                PlayerView.Event.ScoreChange, playerScoreController.OnChange);
            eventBus.AddListener(PlayerScoreView.Event.Construct,
                playerScoreController.OnViewConstruct);
            eventBus.AddListener(LevelView.Event.Construct,
                playerScoreController.OnLevelConstruct);

            var playerStateStorageController =
                injectionBinder.GetInstance<PlayerStateStorageController>();
            eventBus.AddListener(PlayerStateStorage.Event.Save,
                playerStateStorageController.OnSave);
        }
    }
}
