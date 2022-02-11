using System.Globalization;

namespace Translator.ObjectModel
{
    internal sealed class Float : Object
    {
        public Float() { }

        public Float(double value)
        {
            Value = value;
        }

        public override ObjectTypes Type => ObjectTypes.Float;
        public double Value { get; }

        public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);

        public static Float Parse(string value) => 
            new Float(double.Parse(value, NumberStyles.Any, CultureInfo.InvariantCulture));
    }
}
