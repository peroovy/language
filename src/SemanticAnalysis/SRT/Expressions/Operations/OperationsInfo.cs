using System.Collections.Generic;

namespace Translator.SRT
{
    internal static class OperationsInfo
    {
        private static readonly Dictionary<TokenType, UnaryOperation> _unaryOperatorsBindings = 
            new Dictionary<TokenType, UnaryOperation>
        {
            [TokenType.Plus] = UnaryOperation.Positive,
            [TokenType.Minus] = UnaryOperation.Negation
        };

        private static readonly Dictionary<TokenType, BinaryOperation> _binaryOperatorsBindings = 
            new Dictionary<TokenType, BinaryOperation>
        {
            [TokenType.Plus] = BinaryOperation.Addition,
            [TokenType.Minus] = BinaryOperation.Subtraction,
            [TokenType.Star] = BinaryOperation.Multiplication,
            [TokenType.Slash] = BinaryOperation.Division,
            [TokenType.StarStar] = BinaryOperation.Exponentiation
        };

        public static UnaryOperation? ToUnaryOperation(this TokenType type) => 
            GetOperation(type, _unaryOperatorsBindings);

        public static BinaryOperation? ToBinaryOperation(this TokenType type) => 
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
