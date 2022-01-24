using System.Collections.Generic;
using Translator.ObjectModel;

namespace Translator
{
    internal static class ObjectTypesBinder
    {
        private readonly static Dictionary<TokenTypes, ObjectTypes> _bindings = new Dictionary<TokenTypes, ObjectTypes>()
        {
            [TokenTypes.IntKeyword] = ObjectTypes.Int,
            [TokenTypes.FloatKeyword] = ObjectTypes.Float,
            [TokenTypes.BoolKeyword] = ObjectTypes.Bool
        };

        public static ObjectTypes GetVariableType(this TokenTypes keyword) => 
            _bindings.TryGetValue(keyword, out var type) ? type : ObjectTypes.Unknown;
    }
}