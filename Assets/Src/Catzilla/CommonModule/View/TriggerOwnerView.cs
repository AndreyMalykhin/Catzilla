using UnityEngine;
using Catzilla.CommonModule.Util;

namespace Catzilla.CommonModule.View {
    public class TriggerOwnerView: MonoBehaviour, IPoolable {
        [SerializeField]
        private TriggerView[] triggers;

        private int triggersCount;

        void IPoolable.OnReturn() {
            MoveIn();
        }

		void IPoolable.OnTake() {}

        private void Awake() {
            triggersCount = triggers.Length;

            for (int i = 0; i < triggersCount; ++i) {
                triggers[i].Owner = this;
            }
        }

        private void OnEnable() {
            MoveOut();

            for (int i = 0; i < triggersCount; ++i) {
                triggers[i].gameObject.SetActive(true);
            }
        }

        private void OnDisable() {
            for (int i = 0; i < triggersCount; ++i) {
                triggers[i].gameObject.SetActive(false);
            }
        }

        private void OnTransformParentChanged() {
            MoveOut();
        }

        private void MoveOut() {
            for (int i = 0; i < triggersCount; ++i) {
                triggers[i].transform.parent = transform.parent;
            }
        }

        private void MoveIn() {
            for (int i = 0; i < triggersCount; ++i) {
                triggers[i].transform.parent = transform;
            }
        }

        private void LateUpdate() {
            for (int i = 0; i < triggersCount; ++i) {
                TriggerView trigger = triggers[i];
                trigger.transform.position =
                    transform.position + trigger.PositionOffset;
            }
        }
    }
}
