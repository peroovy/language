using System.Collections.Generic;

namespace Translator
{
    internal static class OperatorsBinder
    {
        private static readonly Dictionary<TokenTypes, IUnaryOperation> _unaryOperatorsBindings = 
            new Dictionary<TokenTypes, IUnaryOperation>
        {
            [TokenTypes.Plus] = new Positive(),
            [TokenTypes.Minus] = new Negation(),
            [TokenTypes.Bang] = new LogicalNegation() 
        };

        private static readonly Dictionary<TokenTypes, IBinaryOperation> _binaryOperatorsBindings = 
            new Dictionary<TokenTypes, IBinaryOperation>
        {
            [TokenTypes.Plus] = new Addition(),
            [TokenTypes.Minus] = new Subtraction(),
            [TokenTypes.Star] = new Multiplication(),
            [TokenTypes.Slash] = new Division(),
            [TokenTypes.DoubleStar] = new Exponentiation(),

            [TokenTypes.LeftArrow] = new Less(),
            [TokenTypes.LeftArrowEquals] = new LessOrEquality(),
            [TokenTypes.RightArrow] = new More(),
            [TokenTypes.RightArrowEquals] = new MoreOrEquality(),
            [TokenTypes.DoubleEquals] = new Equality(),
            [TokenTypes.BangEquals] = new NotEquality(),
            [TokenTypes.DoubleOpersand] = new LogicalAnd(),
            [TokenTypes.DoubleVerticalBar] = new LogicalOr()
        };

        public static IUnaryOperation ToUnaryOperation(this TokenTypes type) => 
            GetOperation(type, _unaryOperatorsBindings);

        public static IBinaryOperation ToBinaryOperation(this TokenTypes type) => 
            GetOperation(type, _binaryOperatorsBindings);

        private static TOperation GetOperation<TOperation>(TokenTypes op, Dictionary<TokenTypes, TOperation> bindings)
            where TOperation : class
        {
            if (bindings.TryGetValue(op, out var operation))
                return operation;

            return null;
        }
    }
}
