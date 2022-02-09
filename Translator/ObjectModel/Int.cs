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

        public static Int Create(string value)
        {
            if (!int.TryParse(value, out var number))
                throw new System.OverflowException();

            return new Int(number);
        }
    }
}
