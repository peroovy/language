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
        public Object Value { get; private set; }

        public void SetValue(Object value)
        {
            if (Type != value.Type)
                throw new System.InvalidCastException();

            Value = value;
        }
    }
}
