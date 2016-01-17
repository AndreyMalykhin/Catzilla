using UnityEngine;
using System.Collections;
using strange.extensions.context.api;
using Catzilla.MainMenuModule.View;
using Catzilla.AppModule.Controller;

namespace Catzilla.AppModule.Controller {
    public class AppController {
        [Inject]
        public MainScreenView MainScreen {get;set;}

        public void OnStart() {
            Debug.Log("AppController.OnStart()");
            MainScreen.Show();
        }

        public void OnExit() {
            Debug.Log("AppController.OnExit()");
            Application.Quit();
        }
    }
}
