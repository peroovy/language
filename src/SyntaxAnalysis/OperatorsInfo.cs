namespace Translator
{
    internal static class OperatorsInfo
    {
        public static int? GetBinaryOperatorPrecedence(this TokenType type)
        {
            switch (type)
            {
                case TokenType.Plus:
                case TokenType.Minus:
                    return 1;

                case TokenType.Star:
                case TokenType.Slash:
                    return 2;

                case TokenType.StarStar:
                    return 3;

                default:
                    return null;
            }
        }

        public static bool IsUnaryOperator(this TokenType type)
        {
            return type == TokenType.Plus
                || type == TokenType.Minus;
        }
    }
}
