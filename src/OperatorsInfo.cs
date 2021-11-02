namespace Translator
{
    static class OperatorsInfo
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

                default:
                    return null;
            }
        }
    }
}
