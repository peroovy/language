namespace Translator.AST
{
    internal sealed class SyntaxBinaryExpression : SyntaxExpression
    {
        public SyntaxBinaryExpression(SyntaxExpression left, Token operatorToken, SyntaxExpression right)
        {
            Left = left;
            OperatorToken = operatorToken;
            Right = right;
        }

        public override SyntaxNodeKind Kind => SyntaxNodeKind.BinaryExpression;
        public SyntaxExpression Left { get; }
        public Token OperatorToken { get; }
        public SyntaxExpression Right { get; }
    }
}
