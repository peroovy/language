using System.Collections.Generic;

namespace Translator
{
    internal static class OperatorsBinder
    {
        private static readonly Dictionary<TokenTypes, IUnaryOperation> _unaryOperatorsBindings = 
            new Dictionary<TokenTypes, IUnaryOperation>
        {
            [TokenTypes.Plus] = Positive.Instance,
            [TokenTypes.Minus] = Negation.Instance,
            [TokenTypes.Bang] = LogicalNegation.Instance 
        };

        private static readonly Dictionary<TokenTypes, IBinaryOperation> _binaryOperatorsBindings = 
            new Dictionary<TokenTypes, IBinaryOperation>
        {
            [TokenTypes.Plus] = Addition.Instance,
            [TokenTypes.Minus] = Subtraction.Instance,
            [TokenTypes.Star] = Multiplication.Instance,
            [TokenTypes.Slash] = Division.Instance,
            [TokenTypes.DoubleStar] = Exponentiation.Instance,

            [TokenTypes.LeftArrow] = Less.Instance,
            [TokenTypes.LeftArrowEquals] = LessOrEquality.Instance,
            [TokenTypes.RightArrow] = More.Instance,
            [TokenTypes.RightArrowEquals] = MoreOrEquality.Instance,
            [TokenTypes.DoubleEquals] = Equality.Instance,
            [TokenTypes.BangEquals] = NotEquality.Instance,
            [TokenTypes.DoubleOpersand] = LogicalAnd.Instance,
            [TokenTypes.DoubleVerticalBar] = LogicalOr.Instance
        };

        public static IUnaryOperation ToUnaryOperation(this TokenTypes type) => 
            GetOperation(type, _unaryOperatorsBindings);

        public static IBinaryOperation ToBinaryOperation(this TokenTypes type) => 
            GetOperation(type, _binaryOperatorsBindings);

        private static TOperation GetOperation<TOperation>(TokenTypes op, Dictionary<TokenTypes, TOperation> bindings)
            where TOperation : class
        {
            return bindings.TryGetValue(op, out var operation) ? operation : null;
        }
    }
}
