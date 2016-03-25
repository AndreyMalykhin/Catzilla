using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;

namespace Catzilla.GameOverMenuModule.View {
    public class GameOverScreenView: MonoBehaviour {
        public int AvailableResurrectionsCount {
            get {return availableResurrectionsCount;}
            set {
                availableResurrectionsCount = value;
                RenderResurrectBtn();
            }
        }

        public int AvailableRewardsCount {
            get {return availableRewardsCount;}
            set {
                availableRewardsCount = value;
                RenderRewardBtn();
            }
        }

        public string ResurrectTextTemplate {
            get {return resurrectTextTemplate;}
            set {resurrectTextTemplate = value;}
        }

        public string RewardTextTemplate {
            get {return rewardTextTemplate;}
            set {rewardTextTemplate = value;}
        }

        public Text ResurrectText;
        public Text RewardText;
        public Button ExitBtn;
        public Button RestartBtn;
        public Button ResurrectBtn;
        public Button RewardBtn;

        private int availableResurrectionsCount;
        private string resurrectTextTemplate = "Resurrect ({0})";
        private int availableRewardsCount;
        private string rewardTextTemplate = "Earn reward ({0})";

        private void Awake() {
            // DebugUtils.Log("GameOverMenuView.Awake()");
            RenderResurrectBtn();
            RenderRewardBtn();
        }

        private void RenderResurrectBtn() {
            ResurrectText.text = string.Format(
                resurrectTextTemplate, availableResurrectionsCount);
            ResurrectBtn.interactable = availableResurrectionsCount > 0;
        }

        private void RenderRewardBtn() {
            RewardText.text = string.Format(
                rewardTextTemplate, availableRewardsCount);
            RewardBtn.interactable = availableRewardsCount > 0;
        }
    }
}
