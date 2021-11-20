using System.Collections.Generic;

namespace Translator.SRT
{
    internal static class OperatorsBinder
    {
        private static readonly Dictionary<TokenType, UnaryOperationKind> _unaryOperatorsBindings = 
            new Dictionary<TokenType, UnaryOperationKind>
        {
            [TokenType.Plus] = UnaryOperationKind.Positive,
            [TokenType.Minus] = UnaryOperationKind.Negation,
            [TokenType.Bang] = UnaryOperationKind.LogicalNegation
        };

        private static readonly Dictionary<TokenType, BinaryOperationKind> _binaryOperatorsBindings = 
            new Dictionary<TokenType, BinaryOperationKind>
        {
            [TokenType.Plus] = BinaryOperationKind.Addition,
            [TokenType.Minus] = BinaryOperationKind.Subtraction,
            [TokenType.Star] = BinaryOperationKind.Multiplication,
            [TokenType.Slash] = BinaryOperationKind.Division,
            [TokenType.DoubleStar] = BinaryOperationKind.Exponentiation,
            [TokenType.DoubleOpersand] = BinaryOperationKind.LogicalAnd,
            [TokenType.DoubleVerticalBar] = BinaryOperationKind.LogicalOr
        };

        public static UnaryOperationKind? ToUnaryOperationKind(this TokenType type) => 
            GetOperation(type, _unaryOperatorsBindings);

        public static BinaryOperationKind? ToBinaryOperationKind(this TokenType type) => 
            GetOperation(type, _binaryOperatorsBindings);

        private static TOperation? GetOperation<TOperation>(TokenType op, Dictionary<TokenType, TOperation> bindings)
            where TOperation : struct
        {
            if (bindings.TryGetValue(op, out var operation))
                return operation;

            return null;
        }
    }
}
