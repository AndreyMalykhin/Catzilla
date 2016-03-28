namespace Catzilla.CommonModule.Util {
    public interface IPoolable {
        void OnReturn();
        void OnTake();
    }
}
