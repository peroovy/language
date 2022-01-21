namespace Translator.AST
{
    internal sealed class SyntaxAssignmentExpression : SyntaxExpression
    {
        public SyntaxAssignmentExpression(Token identifier, Token op, SyntaxExpression expression)
        {
            Identifier = identifier;
            Operator = op;
            Expression = expression;
        }

        public override SyntaxNodeKind Kind => SyntaxNodeKind.AssignmentExpression;
        public Token Identifier { get; }
        public Token Operator { get; }
        public SyntaxExpression Expression { get; }
    }
}
