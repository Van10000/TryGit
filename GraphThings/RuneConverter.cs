using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace PudgeClient
{
    class RuneConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                var p = ((string)value).Split(' ');
                if (p[2] == "") return new Rune(double.Parse(p[0]), double.Parse(p[1]));
                var edges = p.Skip(2)
                                .GetBigrams()
                                .ToList()
                                .ConvertAll(coord => new Rune(double.Parse(coord.Item1), double.Parse(coord.Item2)))
                                .GetBigrams()
                                .ToList()
                                .ConvertAll(nodes => new Edge(nodes.Item1, nodes.Item2));
                return new Rune(double.Parse(p[0]), double.Parse(p[1]));
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
                return (((Rune)value).x + " " + ((Rune)value).y + " " + string.Join(" ", ((Rune)value).Edges));
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
