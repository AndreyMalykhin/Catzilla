using UnityEngine;
using System.Collections;

namespace Catzilla.CommonModule.Util {
    public struct EventData {
        public object EventOwner {get; private set;}
        public object Data {get; private set;}

        public EventData(object owner, object data) {
            EventOwner = owner;
            Data = data;
        }
    }
}
