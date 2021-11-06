namespace Translator.AST
{
    internal sealed class UnaryExpression : Expression
    {
        public UnaryExpression(Token operatorToken, Expression operand)
        {
            OperatorToken = operatorToken;
            Operand = operand;
        }

        public override SyntaxNodeKind Kind => SyntaxNodeKind.UnaryExpression;
        public Token OperatorToken { get; }
        public Expression Operand { get; }
    }
}
