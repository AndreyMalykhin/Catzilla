using UnityEngine;
using System;
using System.Collections;
using strange.extensions.pool.api;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using Catzilla.CommonModule.Util;

namespace Catzilla.CommonModule.View {
    public class PoolableView
        : strange.extensions.mediation.impl.View, IPoolable {
        public enum Event {Destroy}

        [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
        public IEventDispatcher EventBus {get; set;}

        public int PoolId;
		public bool retain {get {return false;}}

		public void Restore() {
            var components = GetComponents<MonoBehaviour>();

            for (var i = 0; i < components.Length; ++i) {
                var component = components[i];

                if (component != this && component is IPoolable) {
                    ((IPoolable) component).Restore();
                }
            }
        }

		public void Retain() {Debug.Assert(false);}
		public void Release() {Debug.Assert(false);}

        protected override void OnDestroy() {
            // DebugUtils.Log("PoolableView.OnDestroy()");
            // can be null while app is destroying
            if (EventBus != null) {
                EventBus.Dispatch(Event.Destroy, this);
            }

            base.OnDestroy();
        }
    }
}
