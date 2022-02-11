namespace Translator.ObjectModel
{
    internal sealed class Int : Object
    {
        public Int() { }

        public Int(int value)
        {
            Value = value;
        }

        public Int(double value)
        {
            Value = (int)value;
        }

        public override ObjectTypes Type => ObjectTypes.Int;
        public int Value { get; }

        public override string ToString() => Value.ToString();

        public static Int Parse(string value) => new Int(int.Parse(value));

        public static bool TryParse(string value, out Int obj)
        {
            var is_parsed = int.TryParse(value, out var number);

            obj = is_parsed ? new Int(number) : null;
            return is_parsed;
        }
    }
}
