using System;
using System.Collections.Generic;
using Translator.SRT;

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
            var message = $"ERROR: Unknown token '{name}' in line {location.NumberLine} on position {location.Span.Start}";

            Report(message, location);
        }

        public void ReportUnexpectedTokenError(TokenType actual, TokenType expected, TextLocation location)
        {
            var message = $"ERROR: Expected '{expected}', but was '{actual}' in line {location.NumberLine} on position {location.Span.Start}";

            Report(message, location);
        }

        public void ReportUndefinedUnaryOperationFor(Type type, UnaryOperation operation, TextLocation location)
        {
            var message = $"ERROR: The unary operation '{operation}' is not defined for type '{type}' in line {location.NumberLine} on position {location.Span.Start}";

            Report(message, location);
        }

        public void ReportUndefinedBinaryOperationFor(Type left, Type right, BinaryOperation operation, TextLocation location)
        {
            var message = $"ERROR: The binary operator '{operation}' is not defined for types '{left}' and '{right}' in line {location.NumberLine} on position {location.Span.Start}";

            Report(message, location);
        }

        public void ReportDivisionByZero(TextLocation location)
        {
            var message = $"ERROR: Division by zero in line {location.NumberLine}";

            Report(message, location);
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
