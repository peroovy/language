namespace Translator.AST
{
    internal sealed class SyntaxUnaryExpression : SyntaxExpression
    {
        public SyntaxUnaryExpression(Token operatorToken, SyntaxExpression operand)
        {
            OperatorToken = operatorToken;
            Operand = operand;
        }

        public override SyntaxNodeKind Kind => SyntaxNodeKind.UnaryExpression;
        public Token OperatorToken { get; }
        public SyntaxExpression Operand { get; }
    }
}
