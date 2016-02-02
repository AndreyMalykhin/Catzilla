using UnityEngine;
using System.Collections;
using strange.extensions.injector.api;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using SmartLocalization;
using Catzilla.CommonModule.Config;
using Catzilla.CommonModule.Util;
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
        }

        void IModuleConfig.PostBindings(IInjectionBinder injectionBinder) {
            var eventBus = injectionBinder.GetInstance<EventBus>();
            var playerScoreController =
                injectionBinder.GetInstance<PlayerScoreController>();
            eventBus.On(
                PlayerView.Event.ScoreChange, playerScoreController.OnChange);
            eventBus.On(PlayerScoreView.Event.Construct,
                playerScoreController.OnViewConstruct);
            eventBus.On(LevelView.Event.Construct,
                playerScoreController.OnLevelConstruct);
        }
    }
}
