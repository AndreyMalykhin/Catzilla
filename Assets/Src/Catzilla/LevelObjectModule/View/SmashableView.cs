using UnityEngine;
using System.Collections;
using Zenject;
using Catzilla.CommonModule.Util;
using Catzilla.CommonModule.View;

namespace Catzilla.LevelObjectModule.View {
    public class SmashableView: MonoBehaviour {
        public Transform[] Parts {get {return parts;}}

        [Inject]
        private EventBus eventBus;

        [Inject]
        private PoolStorageView poolStorage;

        [SerializeField]
        private SmashedView smashedProto;

        [SerializeField]
        private Transform[] parts;

        [SerializeField]
        private PoolableView poolable;

        private int smashedPoolId;

        [PostInject]
        public void OnConstruct() {
            // DebugUtils.Log("SmashableView.OnConstruct()");
            smashedPoolId = smashedProto.GetComponent<PoolableView>().PoolId;
        }

        public SmashedView Smash(
            float force, float upwardsModifier, Vector3 sourcePosition) {
            var smashed =
                poolStorage.Take(smashedPoolId).GetComponent<SmashedView>();
            smashed.Init(this, force, upwardsModifier, sourcePosition);
            eventBus.Fire((int) Events.SmashableSmash, new Evt(this, smashed));
            poolStorage.Return(poolable);
            return smashed;
        }
    }
}
