using System;
using System.Collections.Generic;
using System.Linq;

namespace Translator.SRT
{
    internal sealed class UnaryOperation
    {
        private static readonly IReadOnlyList<UnaryOperation> _patterns = new[]
        {
            new UnaryOperation(UnaryOperationKind.Positive, typeof(int)),
            new UnaryOperation(UnaryOperationKind.Negation, typeof(int)),
            new UnaryOperation(UnaryOperationKind.LogicalNegation, typeof(bool))
        };

        public UnaryOperation(UnaryOperationKind kind, Type operandType)
            : this(kind, operandType, operandType)
        { 
        }


        public UnaryOperation(UnaryOperationKind kind, Type operandType, Type returnedType)
        {
            Kind = kind;
            OperandType = operandType;
            ReturnedType = returnedType;
        }

        public UnaryOperationKind Kind { get; }
        public Type OperandType { get; }
        public Type ReturnedType { get; }

        public static UnaryOperation Resolve(TokenType tokenType, ResolvedExpression operand)
        {
            var operationKind = tokenType.ToUnaryOperationKind();
            if (operationKind is null)
                return null;

            var pattern = _patterns.FirstOrDefault(p =>
                    operationKind == p.Kind && operand.ReturnedType == p.OperandType);

            return pattern ?? new UnaryOperation(operationKind.Value, operand.ReturnedType, null);
        }
    }
}
