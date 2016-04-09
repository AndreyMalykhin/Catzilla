namespace Catzilla.LevelObjectModule.Model {
    public struct CriticalValue {
        public readonly int Value;
        public readonly bool IsCritical;

        public CriticalValue(int value, bool isCritical = false) {
            Value = value;
            IsCritical = isCritical;
        }

        public static implicit operator CriticalValue(int value) {
            return new CriticalValue(value);
        }

        public static implicit operator int(CriticalValue value) {
            return value.Value;
        }
    }
}
