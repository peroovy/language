using Translator.AST;

namespace Translator
{
    internal sealed class SyntaxVariableDeclarationStatement : SyntaxStatement
    {
        public SyntaxVariableDeclarationStatement(Token keyword, Token identifier)
        {
            Keyword = keyword;
            Identifier = identifier;
        }

        public SyntaxVariableDeclarationStatement(Token keyword, Token identifier, Token op, SyntaxExpression expression)
        {
            Keyword = keyword;
            Identifier = identifier;
            Operator = op;
            InitializedExpression = expression;
        }

        public override SyntaxNodeKind Kind => SyntaxNodeKind.VariableDeclarationStatement;
        public Token Keyword { get; }
        public Token Identifier { get; }
        public Token Operator { get; }
        public SyntaxExpression InitializedExpression { get; }
    }
}
