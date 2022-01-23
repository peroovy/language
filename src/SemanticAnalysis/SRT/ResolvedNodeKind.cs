namespace Translator.SRT
{
    internal enum ResolvedNodeKind
    {
        LiteralExpression,
        IdentifierExpression,
        ParenthesizedExpression,
        UnaryExpression,
        BinaryExpression,
        AssignmentExpression,
        LostExpression,

        VariableDeclarationStatement,
        LostStatement,
    }
}
