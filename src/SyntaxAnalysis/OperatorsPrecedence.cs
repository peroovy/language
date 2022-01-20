﻿namespace Translator.AST
{
    internal static class OperatorsPrecedence
    {
        public static int? GetUnaryOperatorPrecedence(this TokenTypes type)
        {
            switch (type)
            {
                case TokenTypes.Plus:
                case TokenTypes.Minus:
                    return 8;

                case TokenTypes.Bang:
                    return 4;
            }

            return null;
        }

        public static int? GetBinaryOperatorPrecedence(this TokenTypes type)
        {
            switch (type)
            {
                case TokenTypes.DoubleStar:
                    return 10;

                case TokenTypes.Star:
                case TokenTypes.Slash:
                    return 7;

                case TokenTypes.Plus:
                case TokenTypes.Minus:
                    return 6;

                case TokenTypes.LeftArrow:
                case TokenTypes.LeftArrowEquals:
                case TokenTypes.RightArrow:
                case TokenTypes.RightArrowEquals:
                case TokenTypes.BangEquals:
                case TokenTypes.DoubleEquals:
                    return 5;

                case TokenTypes.DoubleOpersand:
                    return 3;

                case TokenTypes.DoubleVerticalBar:
                    return 2;
            }

            return null;
        }
    }
}
