using System.Globalization;

namespace Translator.ObjectModel
{
    internal sealed class Float : Object
    {
        public Float()
        {
        }

        public Float(double value)
        {
            Value = value;
        }

        public override ObjectTypes Type => ObjectTypes.Float;
        public double Value { get; }

        public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);

        public static Float Create(string value)
        {
            return double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var number)
                ? new Float(number) 
                : null;
        }

        public static Float operator +(Float left, Int right) => new Float(left.Value + right.Value);

        public static Float operator +(Float left, Float right) => new Float(left.Value + right.Value);
        
        public static Float operator -(Float left, Int right) => new Float(left.Value - right.Value);
        
        public static Float operator -(Float left, Float right) => new Float(left.Value - right.Value);
        
        public static Float operator *(Float left, Int right) => new Float(left.Value * right.Value);
        
        public static Float operator *(Float left, Float right) => new Float(left.Value * right.Value);
        
        public static Float operator /(Float left, Int right) => right.Value == 0 ? null : new Float(left.Value / right.Value);
        
        public static Float operator /(Float left, Float right) => right.Value == 0 ? null : new Float(left.Value / right.Value);
        
        public static Bool operator ==(Float left, Float right) => new Bool(left.Value == right.Value);
        
        public static Bool operator !=(Float left, Float right) => new Bool(left.Value != right.Value);
        
        public static Bool operator ==(Float left, Int right) => new Bool(left.Value == right.Value);
        
        public static Bool operator !=(Float left, Int right) => new Bool(left.Value != right.Value);

        public static Bool operator <(Float left, Float right) => new Bool(left.Value < right.Value);

        public static Bool operator >(Float left, Float right) => new Bool(left.Value > right.Value);

        public static Bool operator <(Float left, Int right) => new Bool(left.Value < right.Value);

        public static Bool operator >(Float left, Int right) => new Bool(left.Value > right.Value);

        public static Bool operator <=(Float left, Float right) => new Bool(left.Value <= right.Value);

        public static Bool operator >=(Float left, Float right) => new Bool(left.Value >= right.Value);

        public static Bool operator <=(Float left, Int right) => new Bool(left.Value <= right.Value);

        public static Bool operator >=(Float left, Int right) => new Bool(left.Value >= right.Value);
    }
}
