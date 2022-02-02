using Translator.ObjectModel;

namespace Translator.AST
{
    internal sealed class SyntaxLiteralExpression : SyntaxExpression
    {
        public SyntaxLiteralExpression(Token token, ObjectTypes valueType)
        {
            Token = token;
            ObjectType = valueType;
        }

        public override SyntaxNodeKind Kind => SyntaxNodeKind.LiteralExpression;
        public Token Token { get; }
        public ObjectTypes ObjectType { get; }
    }
}
