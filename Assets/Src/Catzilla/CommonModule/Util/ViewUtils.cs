using UnityEngine;
using System;
using System.Diagnostics;
using System.Collections;
using Zenject;

namespace Catzilla.CommonModule.Util {
    public static class ViewUtils {
        public static void DispatchNowOrAtFixedUpdate(
            MonoBehaviour view,
            Func<EventBus> eventBusProvider,
            object eventId,
            Evt evt) {
            if (eventBusProvider() == null) {
                view.StartCoroutine(DispatchAtFixedUpdate(
                    eventBusProvider, eventId, evt));
                return;
            }

            eventBusProvider().Fire(eventId, evt);
        }

        private static IEnumerator DispatchAtFixedUpdate(
            Func<EventBus> eventBusProvider,
            object eventId,
            Evt evt) {
            yield return new WaitForFixedUpdate();
            eventBusProvider().Fire(eventId, evt);
        }
    }
}
