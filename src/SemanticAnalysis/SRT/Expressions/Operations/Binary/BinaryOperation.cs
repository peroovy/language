using System;
using System.Collections.Generic;
using System.Linq;

namespace Translator.SRT
{
    internal sealed class BinaryOperation
    {
        private static readonly IReadOnlyList<BinaryOperation> _patterns = new[]
        {
            new BinaryOperation(BinaryOperationKind.Addition, typeof(int), typeof(int), typeof(int)),
            new BinaryOperation(BinaryOperationKind.Subtraction, typeof(int), typeof(int), typeof(int)),
            new BinaryOperation(BinaryOperationKind.Multiplication, typeof(int), typeof(int), typeof(int)),
            new BinaryOperation(BinaryOperationKind.Division, typeof(int), typeof(int), typeof(int)),
            new BinaryOperation(BinaryOperationKind.Exponentiation, typeof(int), typeof(int), typeof(int)),
            
            new BinaryOperation(BinaryOperationKind.Less, typeof(int), typeof(int), typeof(bool)),
            new BinaryOperation(BinaryOperationKind.LessOrEquals, typeof(int), typeof(int), typeof(bool)),
            new BinaryOperation(BinaryOperationKind.More, typeof(int), typeof(int), typeof(bool)),
            new BinaryOperation(BinaryOperationKind.MoreOrEquals, typeof(int), typeof(int), typeof(bool)),
            new BinaryOperation(BinaryOperationKind.Equality, typeof(int), typeof(int), typeof(bool)),
            new BinaryOperation(BinaryOperationKind.NotEquality, typeof(int), typeof(int), typeof(bool)),
            new BinaryOperation(BinaryOperationKind.LogicalAnd, typeof(bool), typeof(bool), typeof(bool)),
            new BinaryOperation(BinaryOperationKind.LogicalOr, typeof(bool), typeof(bool), typeof(bool)),                                                                                              
        };

        public BinaryOperation(BinaryOperationKind kind, Type leftType, Type rightType, Type returnedType)
        {
            Kind = kind;
            LeftType = leftType;
            RightType = rightType;
            ReturnedType = returnedType;
        }

        public BinaryOperationKind Kind { get; }
        public Type LeftType { get; }
        public Type RightType { get; }
        public Type ReturnedType { get; }

        public static BinaryOperation Resove(TokenType token, ResolvedExpression left, ResolvedExpression right)
        {
            var operationKind = token.ToBinaryOperationKind();
            if (operationKind is null)
                return null;

            var pattern = _patterns.FirstOrDefault(p => 
                    operationKind == p.Kind && left.ReturnedType == p.LeftType && right.ReturnedType == p.RightType);

            return pattern ?? new BinaryOperation(operationKind.Value, left.ReturnedType, right.ReturnedType, null);
        }
    }
}
