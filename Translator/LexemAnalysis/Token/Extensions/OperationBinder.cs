using System.Collections.Generic;

namespace Translator
{
    internal static class OperationBinder
    {
        private static readonly Dictionary<TokenTypes, UnaryOperation> _unaryOperationBindings = 
            new Dictionary<TokenTypes, UnaryOperation>
            {
                [TokenTypes.Plus] = Positive.Instance,
                [TokenTypes.Minus] = Negation.Instance,
                [TokenTypes.Bang] = LogicalNegation.Instance 
            };

        private static readonly Dictionary<TokenTypes, BinaryOperation> _binaryOperationBindings = 
            new Dictionary<TokenTypes, BinaryOperation>
            {
                [TokenTypes.Plus] = Addition.Instance,
                [TokenTypes.Minus] = Subtraction.Instance,
                [TokenTypes.Star] = Multiplication.Instance,
                [TokenTypes.Slash] = Division.Instance,
                [TokenTypes.DoubleStar] = Exponentiation.Instance,

                [TokenTypes.PlusEquals] = Addition.Instance,
                [TokenTypes.MinusEquals] = Subtraction.Instance,
                [TokenTypes.StarEquals] = Multiplication.Instance,
                [TokenTypes.SlashEquals] = Division.Instance,
                [TokenTypes.OpersandEquals] = LogicalAnd.Instance,
                [TokenTypes.VerticalBarEquals] = LogicalOr.Instance,

                [TokenTypes.LeftArrow] = Less.Instance,
                [TokenTypes.LeftArrowEquals] = LessOrEquality.Instance,
                [TokenTypes.RightArrow] = Greater.Instance,
                [TokenTypes.RightArrowEquals] = GreaterOrEquality.Instance,
                [TokenTypes.DoubleEquals] = Equality.Instance,
                [TokenTypes.BangEquals] = NotEquality.Instance,
                [TokenTypes.DoubleOpersand] = LogicalAnd.Instance,
                [TokenTypes.DoubleVerticalBar] = LogicalOr.Instance
            };

        public static UnaryOperation ToUnaryOperation(this TokenTypes type) => 
            GetOperation(type, _unaryOperationBindings);

        public static BinaryOperation ToBinaryOperation(this TokenTypes type) => 
            GetOperation(type, _binaryOperationBindings);

        private static TOperation GetOperation<TOperation>(TokenTypes op, Dictionary<TokenTypes, TOperation> bindings)
            where TOperation : class
        {
            return bindings.TryGetValue(op, out var operation) ? operation : null;
        }
    }
}
