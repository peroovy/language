namespace Translator.AST
{
    internal enum SyntaxNodeKind
    {
        LiteralExpression,
        IdentifierExpression,
        ParenthesizedExpression,
        UnaryExpression,
        BinaryExpression,
        AssignmentExpression,
        ExplicitCastExpression,

        VariableDeclarationStatement,
    }
}
