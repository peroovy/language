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
            if (ImplicitCasting.Instance.IsApplicable(obj.Type, to))
                return ImplicitCasting.Instance.Apply(obj, to);

            if (obj.Type == ObjectTypes.Float && to == ObjectTypes.Int)
                return new Int((obj as Float).Value);

            throw new System.InvalidCastException();
        }

        public bool IsApplicable(ObjectTypes from, ObjectTypes to)
        {
            return ImplicitCasting.Instance.IsApplicable(from, to)
                || from == ObjectTypes.Float && to == ObjectTypes.Int;
        }
    }
}
