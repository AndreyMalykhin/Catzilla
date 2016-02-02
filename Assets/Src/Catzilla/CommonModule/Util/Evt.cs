namespace Catzilla.CommonModule.Util {
    public struct Evt {
        public object Source {get; private set;}
        public object Data {get; private set;}

        public Evt(object source, object data = null) {
            Source = source;
            Data = data;
        }
    }
}
