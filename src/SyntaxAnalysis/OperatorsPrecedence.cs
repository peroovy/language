namespace Translator.AST
{
    internal static class OperatorsPrecedence
    {
        public static int? GetUnaryOperatorPrecedence(this TokenType type)
        {
            switch (type)
            {
                case TokenType.Plus:
                case TokenType.Minus:
                case TokenType.Bang:
                    return 8;
            }

            return null;
        }

        public static int? GetBinaryOperatorPrecedence(this TokenType type)
        {
            switch (type)
            {
                case TokenType.DoubleStar:
                    return 9;

                case TokenType.Star:
                case TokenType.Slash:
                    return 7;

                case TokenType.Plus:
                case TokenType.Minus:
                    return 6;

                case TokenType.DoubleOpersand:
                    return 4;

                case TokenType.DoubleVerticalBar:
                    return 3;
            }

            return null;
        }
    }
}
