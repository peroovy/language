namespace Translator.ObjectModel
{
    internal class Object
    {
        public virtual ObjectTypes Kind => ObjectTypes.Object;

        public override string ToString() => Kind.ToString();
    }
}
