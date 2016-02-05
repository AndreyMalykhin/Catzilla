﻿using UnityEngine;
using System.Collections;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;
using Catzilla.LevelObjectModule.View;

namespace Catzilla.LevelObjectModule.Controller {
    public class ScoreableController {
        [Inject("PlayerMeshTag")]
        public string PlayerTag {get; set;}

        [Inject]
        public PoolStorageView PoolStorage {get; set;}

        [Inject("ScorePopupProto")]
        public WorldSpacePopupView ScorePopupProto {get; set;}

        [Inject]
        public PopupManager PopupManager {get; set;}

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
            int poolId = ScorePopupProto.GetComponent<PoolableView>().PoolId;
            var popup =
                PoolStorage.Get(poolId).GetComponent<WorldSpacePopupView>();
            popup.PlaceAbove(scoreable.Collider.bounds);
            popup.LookAtTarget = player.MainCamera;
            popup.Msg.text = string.Format("+{0}", scoreable.Score);
            popup.OnHide = OnScorePopupHide;
            PopupManager.Show(popup);
        }

        private void OnScorePopupHide(WorldSpacePopupView popup) {
            PoolStorage.Return(popup.GetComponent<PoolableView>());
        }
    }
}
