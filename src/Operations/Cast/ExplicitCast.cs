using Translator.ObjectModel;

namespace Translator
{
    internal sealed class ExplicitCast : ICastOperation
    {
        private ExplicitCast() { }

        static ExplicitCast()
        {
            Instance = new ExplicitCast();
        }

        public static ExplicitCast Instance { get; }

        public Object CastTo(ObjectTypes type, Object obj)
        {
            if (obj.Type == type)
            {
                switch (obj.Type)
                {
                    case ObjectTypes.Object:
                    case ObjectTypes.Null:
                        return obj;

                    case ObjectTypes.Int:
                        return new Int((obj as Int).Value);

                    case ObjectTypes.Float:
                        return new Float((obj as Float).Value);

                    case ObjectTypes.Bool:
                        return new Bool((obj as Bool).Value);
                }

                throw new System.Exception($"Unknown type {obj.Type}");
            }

            if (obj.Type == ObjectTypes.Int && type == ObjectTypes.Float)
                return new Float((obj as Int).Value);

            if (obj.Type == ObjectTypes.Float && type == ObjectTypes.Int)
                return new Int((obj as Float).Value);

            throw new System.InvalidCastException();
        }

        public bool IsApplicable(ObjectTypes from, ObjectTypes to)
        {
            return from == to
                || from == ObjectTypes.Int && to == ObjectTypes.Float
                || from == ObjectTypes.Float && to == ObjectTypes.Int;
        }
    }
}
