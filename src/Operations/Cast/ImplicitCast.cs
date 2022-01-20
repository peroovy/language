using Translator.ObjectModel;

namespace Translator
{
    internal class ImplicitCast : ICastOperation
    {
        static ImplicitCast()
        {
            Instance = new ImplicitCast();
        }

        public static ImplicitCast Instance { get; }

        public Object CastTo(ObjectTypes to, Object obj)
        {
            if (obj.Type == ObjectTypes.Int && to == ObjectTypes.Float)
                return new Float((obj as Int).Value);

            throw new System.InvalidCastException();
        }

        public bool IsApplicable(ObjectTypes from, ObjectTypes to)
        {
            return from == ObjectTypes.Int && to == ObjectTypes.Float;
        }
    }
}
