namespace Translator.ObjectModel
{
    internal sealed class Variable
    {
        public Variable(string name, ObjectTypes type)
        {
            Name = name;
            Type = type;
        }

        public string Name { get; }
        public ObjectTypes Type { get; }

        public override int GetHashCode() => Name.GetHashCode();

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Variable))
                return false;

            return GetHashCode() == obj.GetHashCode();
        }
    }
}
