using Translator.ObjectModel;

namespace Translator
{
    internal class ImplicitCasting : IConvertibleOperation
    {
        private ImplicitCasting() { }

        static ImplicitCasting()
        {
            Instance = new ImplicitCasting();
        }

        public static ImplicitCasting Instance { get; }

        public Object Apply(Object obj, ObjectTypes to)
        {
            if (obj.Type == to)
                return obj;

            if (obj.Type == ObjectTypes.Int && to == ObjectTypes.Float)
                return new Float((obj as Int).Value);

            if (obj.Type == ObjectTypes.Int && to == ObjectTypes.Long)
                return Long.Create((obj as Int).Value);

            throw new System.InvalidCastException();
        }

        public bool IsApplicable(ObjectTypes from, ObjectTypes to)
        {
            return from == to
                || from == ObjectTypes.Int && to == ObjectTypes.Float
                || from == ObjectTypes.Int && to == ObjectTypes.Long;
        }
    }
}
