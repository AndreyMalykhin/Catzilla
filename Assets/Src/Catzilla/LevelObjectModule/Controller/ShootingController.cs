using UnityEngine;
using System.Collections.Generic;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.LevelObjectModule.View;

namespace Catzilla.LevelObjectModule.Controller {
    public class ShootingController {
        private PlayerView player;
        private readonly List<ShootingView> shootersWithoutTarget =
            new List<ShootingView>(16);

        public void OnPlayerReady(IEvent evt) {
            player = (PlayerView) evt.data;
            ProcessShootersWithoutTarget();
        }

        public void OnViewReady(IEvent evt) {
            SetTarget((ShootingView) evt.data);
        }

        private void SetTarget(ShootingView shooter) {
            shootersWithoutTarget.Add(shooter);

            if (player == null) {
                return;
            }

            ProcessShootersWithoutTarget();
        }

        private void ProcessShootersWithoutTarget() {
            for (int i = 0; i < shootersWithoutTarget.Count; ++i) {
                if (shootersWithoutTarget[i] == null) {
                    continue;
                }

                shootersWithoutTarget[i].Target = player.Collider;
            }

            shootersWithoutTarget.Clear();
        }
    }
}
