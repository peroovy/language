using Translator.AST;

namespace Translator
{
    internal sealed class SyntaxDeclareVariableStatement : SyntaxStatement
    {
        public SyntaxDeclareVariableStatement(Token keyword, Token identifier)
        {
            Keyword = keyword;
            Identifier = identifier;
        }

        public SyntaxDeclareVariableStatement(Token keyword, Token identifier, Token op, SyntaxExpression expression)
        {
            Keyword = keyword;
            Identifier = identifier;
            Operator = op;
            InitializedExpression = expression;
        }

        public override SyntaxNodeKind Kind => SyntaxNodeKind.DeclareVariableStatement;
        public Token Keyword { get; }
        public Token Identifier { get; }
        public Token Operator { get; }
        public SyntaxExpression InitializedExpression { get; }
    }
}
