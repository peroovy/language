namespace Translator.ObjectModel
{
    internal sealed class Bool : Object
    {
        public Bool() { }

        public Bool(bool value)
        {
            Value = value;
        }

        public override ObjectTypes Type => ObjectTypes.Bool;
        public bool Value { get; }

        public override string ToString() => Value ? "true" : "false";

        public static Bool Parse(string value) => new Bool(bool.Parse(value));
    }
}
