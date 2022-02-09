using Translator.ObjectModel;

namespace Translator
{
    internal interface IConvertibleOperation
    {
        bool IsApplicable(ObjectTypes from, ObjectTypes to);

        Object Apply(Object obj, ObjectTypes to);
    }
}
