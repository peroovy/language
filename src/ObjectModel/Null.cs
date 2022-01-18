namespace Translator.ObjectModel
{
    internal sealed class Null : Object
    {
        public override ObjectTypes Kind => ObjectTypes.Null;

        public override string ToString() => "null";
    }
}
