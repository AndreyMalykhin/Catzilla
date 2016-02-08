using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.Text;
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

            int poolId = ScorePopupProto.GetComponent<PoolableView>().PoolId;
            var popup =
                PoolStorage.Take(poolId).GetComponent<WorldSpacePopupView>();
            popup.PlaceAbove(scoreable.Collider.bounds);
            popup.LookAtTarget = player.Camera;
            popup.Msg.text =
                strBuilder.Append('+').Append(scoreable.Score).ToString();
            strBuilder.Length = 0;
            popup.OnHide = OnScorePopupHide;
            PopupManager.Show(popup);
        }

        private void OnScorePopupHide(WorldSpacePopupView popup) {
            PoolStorage.Return(popup.GetComponent<PoolableView>());
        }
    }
}
