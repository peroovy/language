using Translator.ObjectModel;

namespace Translator
{
    internal interface ICastOperation
    {
        bool IsApplicable(ObjectTypes from, ObjectTypes to);

        Object CastTo(ObjectTypes type, Object obj);
    }
}
