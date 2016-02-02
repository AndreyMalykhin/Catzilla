using UnityEngine;
using System.Collections;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.Util;
using Catzilla.LevelObjectModule.View;

namespace Catzilla.LevelObjectModule.Controller {
    public class ProjectileController {
        [Inject("PlayerMeshTag")]
        public string PlayerTag {get; set;}

        public void OnTriggerEnter(Evt evt) {
            var collider = (Collider) evt.Data;

            if (collider == null || !collider.CompareTag(PlayerTag)) {
                return;
            }

            var projectile = (ProjectileView) evt.Source;
            projectile.Hit();
        }
    }
}
