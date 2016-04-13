namespace Catzilla.LevelObjectModule.Model {
    public struct CriticalValue {
        public int Value;
        public bool IsCritical;

        public static implicit operator CriticalValue(int value) {
            return new CriticalValue{Value = value};
        }

        public static implicit operator int(CriticalValue value) {
            return value.Value;
        }
    }
}
