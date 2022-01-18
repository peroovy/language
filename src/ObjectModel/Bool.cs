namespace Translator.ObjectModel
{
    internal sealed class Bool : Object
    {
        public Bool(bool value)
        {
            Value = value;
        }

        public override ObjectTypes Kind => ObjectTypes.Bool;
        public bool Value { get; }

        public override string ToString() => Value ? "true" : "false";

        public static Bool Create(string value)
        {
            return bool.TryParse(value, out var boolean) ? new Bool(boolean) : null;
        }

        public static Bool operator !(Bool operand) => new Bool(!operand.Value);

        public static Bool operator ==(Bool left, Bool right) => new Bool(left.Value == right.Value);

        public static Bool operator !=(Bool left, Bool right) => new Bool(left.Value != right.Value);
    }
}
