namespace Translator.AST
{
    internal sealed class UnaryExpression : Expression
    {
        public UnaryExpression(Token operatorToken, Expression operand)
        {
            OperatorToken = operatorToken;
            Operand = operand;
        }

        public Token OperatorToken { get; }
        public Expression Operand { get; }
    }
}
