using System.Collections.Generic;

namespace Catzilla.CommonModule.Util {
    public class EventBus {
        public delegate void Listener(Evt evt);

        private readonly IDictionary<int, List<Listener>> eventListeners =
            new Dictionary<int, List<Listener>>(64);

        public void Fire(int eventId, Evt evt) {
            List<Listener> listeners = null;

            if (!eventListeners.TryGetValue(eventId, out listeners)) {
                return;
            }

            for (int i = 0; i < listeners.Count; ++i) {
                listeners[i](evt);
            }
        }

        public void On(int eventId, Listener listener) {
            List<Listener> listeners = null;

            if (!eventListeners.TryGetValue(eventId, out listeners)) {
                listeners = new List<Listener>(4);
                eventListeners[eventId] = listeners;
            }

            listeners.Add(listener);
        }
    }
}
