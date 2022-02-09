using System.Collections.Immutable;
using Translator.ObjectModel;

namespace Translator
{
    internal sealed class ExplicitCasting : IConvertibleOperation
    {
        private ExplicitCasting() { }

        static ExplicitCasting()
        {
            Instance = new ExplicitCasting();
        }

        public static ExplicitCasting Instance { get; }

        public Object Apply(Object obj, ObjectTypes to)
        {
            if (obj.Type == to)
                return obj;

            if (obj.Type == ObjectTypes.Int && to == ObjectTypes.Float)
                return new Float((obj as Int).Value);

            if (obj.Type == ObjectTypes.Float && to == ObjectTypes.Int)
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
