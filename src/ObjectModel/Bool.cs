namespace Translator.ObjectModel
{
    internal sealed class Bool : Object
    {
        public Bool()
        {
        }

        public Bool(bool value)
        {
            Value = value;
        }

        public override ObjectTypes Type => ObjectTypes.Bool;
        public bool Value { get; }

        public override string ToString() => Value ? "true" : "false";

        public override int GetHashCode() => base.GetHashCode();

        public override bool Equals(object obj)
        {
            if (obj is Bool boolean)
                return Value == boolean.Value;

            return false;
        }

        public static Bool Create(string value)
        {
            return bool.TryParse(value, out var boolean) ? new Bool(boolean) : null;
        }

        public static Bool operator !(Bool operand) => new Bool(!operand.Value);

        public static Bool operator ==(Bool left, Bool right) => new Bool(left.Value == right.Value);

        public static Bool operator !=(Bool left, Bool right) => new Bool(left.Value != right.Value);
    }
}
