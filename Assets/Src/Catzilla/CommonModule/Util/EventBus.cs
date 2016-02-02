using System.Collections.Generic;

namespace Catzilla.CommonModule.Util {
    public class EventBus {
        public delegate void Listener(Evt evt);

        private readonly IDictionary<object, List<Listener>> eventListeners =
            new Dictionary<object, List<Listener>>();

        public void Fire(object eventId, Evt evt) {
            List<Listener> listeners;

            if (!eventListeners.TryGetValue(eventId, out listeners)) {
                return;
            }

            for (int i = 0; i < listeners.Count; ++i) {
                listeners[i](evt);
            }
        }

        public void On(object eventId, Listener listener) {
            List<Listener> listeners;

            if (!eventListeners.TryGetValue(eventId, out listeners)) {
                listeners = new List<Listener>(4);
                eventListeners[eventId] = listeners;
            }

            listeners.Add(listener);
        }
    }
}
