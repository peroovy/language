using System.Collections.Generic;

namespace Translator
{
    internal static class OperatorsBinder
    {
        private static readonly Dictionary<TokenType, IUnaryOperation> _unaryOperatorsBindings = 
            new Dictionary<TokenType, IUnaryOperation>
        {
            [TokenType.Plus] = new Positive(),
            [TokenType.Minus] = new Negation(),
            [TokenType.Bang] = new LogicalNegation() 
        };

        private static readonly Dictionary<TokenType, IBinaryOperation> _binaryOperatorsBindings = 
            new Dictionary<TokenType, IBinaryOperation>
        {
            [TokenType.Plus] = new Addition(),
            [TokenType.Minus] = new Subtraction(),
            [TokenType.Star] = new Multiplication(),
            [TokenType.Slash] = new Division(),
            [TokenType.DoubleStar] = new Exponentiation(),

            [TokenType.LeftArrow] = new Less(),
            [TokenType.LeftArrowEquals] = new LessOrEquality(),
            [TokenType.RightArrow] = new More(),
            [TokenType.RightArrowEquals] = new MoreOrEquality(),
            [TokenType.DoubleEquals] = new Equality(),
            [TokenType.BangEquals] = new NotEquality(),
            [TokenType.DoubleOpersand] = new LogicalAnd(),
            [TokenType.DoubleVerticalBar] = new LogicalOr()
        };

        public static IUnaryOperation ToUnaryOperation(this TokenType type) => 
            GetOperation(type, _unaryOperatorsBindings);

        public static IBinaryOperation ToBinaryOperation(this TokenType type) => 
            GetOperation(type, _binaryOperatorsBindings);

        private static TOperation GetOperation<TOperation>(TokenType op, Dictionary<TokenType, TOperation> bindings)
            where TOperation : class
        {
            if (bindings.TryGetValue(op, out var operation))
                return operation;

            return null;
        }
    }
}
