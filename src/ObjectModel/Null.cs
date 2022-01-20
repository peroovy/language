namespace Translator.ObjectModel
{
    internal sealed class Null : Object
    {
        public override ObjectTypes Type => ObjectTypes.Null;

        public override string ToString() => "null";
    }
}
