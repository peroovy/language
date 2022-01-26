using System.Collections.Generic;
using Translator.ObjectModel;

namespace Translator
{
    internal sealed class Diagnostic
    {
        private readonly SourceCode _code;
        private readonly List<Error> _errors = new List<Error>();

        public IEnumerable<Error> Errors => _errors;

        public Diagnostic(SourceCode code)
        {
            _code = code;
        }

        public void ReportUnknownTokenError(string name, TextLocation location)
        {
            var message = $"ERROR({location.NumberLine}, {location.Span.Start}): Unknown token '{name}'";

            Report(message, location);
        }

        public void ReportUnexpectedTokenError(TokenTypes actual, TokenTypes expected, TextLocation location)
        {
            var message = $"ERROR({location.NumberLine}, {location.Span.Start}): Expected '{expected}', but was '{actual}'";

            Report(message, location);
        }

        public void ReportUndefinedUnaryOperationForType(UnaryOperationKind operation, ObjectTypes operand, TextLocation location)
        {
            var message = $"ERROR({location.NumberLine}, {location.Span.Start}): The unary operation '{operation}' is not defined for type '{operand}'";

            Report(message, location);
        }

        public void ReportUndefinedBinaryOperationForTypes(ObjectTypes left, BinaryOperationKind operation, ObjectTypes right, TextLocation operatorLocation)
        {
            var message = $"ERROR({operatorLocation.NumberLine}, {operatorLocation.Span.Start}): The binary operation '{operation}' is not defined for types '{left}' and '{right}'";

            Report(message, operatorLocation);
        }

        public void ReportDivisionByZero(TextLocation location)
        {
            var message = $"ERROR({location.NumberLine}, {location.Span.Start}): Division by zero";

            Report(message, location);
        }

        public void ReportFractionLost(TextLocation location)
        {
            var message = $"ERROR({location.NumberLine}, {location.Span.Start}): Fraction part of number is lost";

            Report(message, location);
        }

        public void ReportUndefinedTypeError(string name, TextLocation location)
        {
            var message = $"ERROR({location.NumberLine}, {location.Span.Start}): Undefined type '{name}'";

            Report(message, location);
        }

        public void ReportVariableAlreadyExistError(string name, TextLocation location)
        {
            var message = $"ERROR({location.NumberLine}, {location.Span.Start}): Variable '{name}' already exists";

            Report(message, location);
        }

        public void ReportImpossibleImplicitCast(ObjectTypes from,  ObjectTypes to, TextLocation equalsLocation)
        {
            var message = $"ERROR({equalsLocation.NumberLine}, {equalsLocation.Span.Start}): Type '{from}' cannot be converted implicitly to '{to}'";

            Report(message, equalsLocation);
        }

        public void ReportUndefinedVariableError(string identifier, TextLocation location)
        {
            var message = $"ERROR({location.NumberLine}, {location.Span.Start}): Variable '{identifier}' doesn's exist";

            Report(message, location);
        }

        public void ReportCannotAssignValueError(TextLocation equalsLocation)
        {
            var message = $"ERROR({equalsLocation.NumberLine}, {equalsLocation.Span.Start}): Сannot be assigned a value";

            Report(message, equalsLocation);
        }

        private void Report(string message, TextLocation location)
        {
            var line = _code.Lines[location.NumberLine];

            if (location.Span.Start >= line.Length)
                line += new string(' ', location.Span.Start - line.Length + 1);

            var error = new Error(message, line, location.Span);

            _errors.Add(error);
        }
    }
}
