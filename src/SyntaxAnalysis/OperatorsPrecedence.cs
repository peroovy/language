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
                    return 8;

                case TokenType.Bang:
                    return 4;
            }

            return null;
        }

        public static int? GetBinaryOperatorPrecedence(this TokenType type)
        {
            switch (type)
            {
                case TokenType.DoubleStar:
                    return 10;

                case TokenType.Star:
                case TokenType.Slash:
                    return 7;

                case TokenType.Plus:
                case TokenType.Minus:
                    return 6;

                case TokenType.LeftArrow:
                case TokenType.LeftArrowEquals:
                case TokenType.RightArrow:
                case TokenType.RightArrowEquals:
                case TokenType.BangEquals:
                case TokenType.DoubleEquals:
                    return 5;

                case TokenType.DoubleOpersand:
                    return 3;

                case TokenType.DoubleVerticalBar:
                    return 2;
            }

            return null;
        }
    }
}
