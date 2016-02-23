﻿using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.Text;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;
using Catzilla.LevelObjectModule.View;

namespace Catzilla.LevelObjectModule.Controller {
    public class ScoreableController {
        [Inject("PlayerMeshTag")]
        public string PlayerTag {get; set;}

        [Inject]
        public WorldSpacePopupManager PopupManager {get; set;}

        private readonly StringBuilder strBuilder = new StringBuilder(8);

        public void OnTriggerEnter(Evt evt) {
            var collider = (Collider) evt.Data;
            // DebugUtils.Log(
            //     "ScoreableController.OnTriggerEnter(); collider={0}", collider);

            if (collider == null || !collider.CompareTag(PlayerTag)) {
                return;
            }

            var player =
                collider.attachedRigidbody.GetComponent<PlayerView>();
            var scoreable = (ScoreableView) evt.Source;
            player.Score += scoreable.Score;
            ShowScorePopup(scoreable, player);
        }

        private void ShowScorePopup(
            ScoreableView scoreable, PlayerView player) {
            if (player.IsScoreFreezed) {
                return;
            }

            WorldSpacePopupView popup = PopupManager.Get();
            popup.PlaceAbove(scoreable.Collider.bounds);
            popup.LookAtTarget = player.Camera;
            popup.Msg.text =
                strBuilder.Append('+').Append(scoreable.Score).ToString();
            strBuilder.Length = 0;
            PopupManager.Show(popup);
        }
    }
}
