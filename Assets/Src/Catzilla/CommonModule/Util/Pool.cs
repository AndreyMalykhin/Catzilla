using System.Collections.Generic;

namespace Catzilla.CommonModule.Util {
    public class Pool<T> where T: IPoolable {
        public interface IInstanceProvider {
            T Get();
        }

        public int Size {get {return instances.Count;}}
        public int Capacity {get; private set;}

        private readonly IInstanceProvider instanceProvider;
        private readonly List<T> instances;

        public Pool(IInstanceProvider instanceProvider, int size) {
            this.instanceProvider = instanceProvider;
            instances = new List<T>(size);
            Capacity = size;
            Add(size);
        }

        public void Return(T instance) {
            instance.OnReturn();
            instances.Add(instance);
        }

        public T Take() {
            if (instances.Count == 0) {
                // DebugUtils.Log("Pool.Take(); empty");
                Add(Capacity);
            }

            int topIndex = instances.Count - 1;
            T instance = instances[topIndex];
            instances.RemoveAt(topIndex);
            instance.OnTake();
            return instance;
        }

        public void Add(int count) {
            for (int i = 0; i < count; ++i) {
                instances.Add(instanceProvider.Get());
            }

            if (instances.Count > Capacity) {
                Capacity = instances.Count * 2;
            }
        }

        public void Clear() {
            instances.Clear();
        }
    }
}
