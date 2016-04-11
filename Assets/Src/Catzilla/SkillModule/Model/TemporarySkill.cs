using UnityEngine;
using System;

namespace Catzilla.SkillModule.Model {
    public class TemporarySkill {
        public float Chance;

        [Tooltip("In seconds")]
        public float Duration;

        private MonoBehaviour view;
        private Action activator;
        private string deactivator;

        public TemporarySkill(MonoBehaviour view,
            Action activator, string deactivator) {
            this.view = view;
            this.activator = activator;
            this.deactivator = deactivator;
        }

        public void Trigger() {
            if (Chance != 1f && UnityEngine.Random.value > Chance) {
                return;
            }

            if (view.IsInvoking(deactivator)) {
                view.CancelInvoke(deactivator);
            } else {
                activator();
            }

            view.Invoke(deactivator, Duration);
        }
    }
}
