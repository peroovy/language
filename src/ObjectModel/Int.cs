namespace Translator.ObjectModel
{
    internal sealed class Int : Object
    {
        public Int()
        {
        }

        public Int(int value)
        {
            Value = value;
        }

        public Int(double value)
        {
            Value = (int)value;
        }

        public override ObjectTypes Type => ObjectTypes.Int;
        public int Value { get; }

        public override string ToString() => Value.ToString();

        public static Int Create(string value)
        {
            return int.TryParse(value, out var number)
                ? new Int(number)
                : null;
        }

        public static Int operator +(Int operand) => new Int(operand.Value);

        public static Int operator -(Int operand) => new Int(-operand.Value);

        public static Int operator +(Int left, Int right) => new Int(left.Value + right.Value);

        public static Float operator +(Int left, Float right) => new Float(left.Value + right.Value);

        public static Int operator -(Int left, Int right) => new Int(left.Value - right.Value);

        public static Float operator -(Int left, Float right) => new Float(left.Value - right.Value);

        public static Int operator *(Int left, Int right) => new Int(left.Value * right.Value);

        public static Float operator *(Int left, Float right) => new Float(left.Value * right.Value);

        public static Int operator /(Int left, Int right) => right.Value == 0 ? null : new Int(left.Value / right.Value);

        public static Float operator /(Int left, Float right) => right.Value == 0 ? null : new Float(left.Value / right.Value);

        public static Bool operator ==(Int left, Int right) => new Bool(left.Value == right.Value);

        public static Bool operator !=(Int left, Int right) => new Bool(left.Value != right.Value);

        public static Bool operator ==(Int left, Float right) => new Bool(left.Value == right.Value);

        public static Bool operator !=(Int left, Float right) => new Bool(left.Value != right.Value);

        public static Bool operator <(Int left, Int right) => new Bool(left.Value < right.Value);

        public static Bool operator >(Int left, Int right) => new Bool(left.Value > right.Value);

        public static Bool operator <(Int left, Float right) => new Bool(left.Value < right.Value);

        public static Bool operator >(Int left, Float right) => new Bool(left.Value > right.Value);

        public static Bool operator <=(Int left, Int right) => new Bool(left.Value <= right.Value);

        public static Bool operator >=(Int left, Int right) => new Bool(left.Value >= right.Value);

        public static Bool operator <=(Int left, Float right) => new Bool(left.Value <= right.Value);

        public static Bool operator >=(Int left, Float right) => new Bool(left.Value >= right.Value);
    }
}
