namespace Translator.ObjectModel
{
    internal class Object
    {
        public virtual ObjectTypes Type => ObjectTypes.Object;

        public override string ToString() => Type.ToString();

        public static Object Create(ObjectTypes type)
        {
            switch (type)
            {
                case ObjectTypes.Object:
                    return new Object();

                case ObjectTypes.Int:
                    return new Int();

                case ObjectTypes.Float:
                    return new Float();

                case ObjectTypes.Bool:
                    return new Bool();

                case ObjectTypes.Null:
                    return new Null();
            }

            throw new System.Exception($"Unknown '{type}' is not created");
        }
    }
}
