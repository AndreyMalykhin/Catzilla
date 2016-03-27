using UnityEngine;
using Zenject;
using Catzilla.CommonModule.Util;

namespace Catzilla.LevelObjectModule.View {
    public class SmashingView: MonoBehaviour {
        public float MinSmashForce {get {return minSmashForce;}}
        public float MaxSmashForce {get {return maxSmashForce;}}
        public float SmashUpwardsModifier {get {return smashUpwardsModifier;}}

        [Inject]
        private EventBus eventBus;

        [SerializeField]
        private float minSmashForce = 96f;

        [SerializeField]
        private float maxSmashForce = 128f;

        [SerializeField]
        private float smashUpwardsModifier;
    }
}
