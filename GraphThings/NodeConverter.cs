using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;


namespace PudgeClient
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<Tuple<T, T>> GetBigrams<T>(this IEnumerable<T> items)
        {
            return items.Select((n, i) => new { GroupNumber = i / 2, Number = n })
                        .GroupBy(n => n.GroupNumber)
                        .Select(g => g.Select(n => n.Number).ToList())
                        .ToList()
                        .Select(l => Tuple.Create(l[0], l[1]));
        }
    }

    class NodeConverter : TypeConverter
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
                if (p[2] == "") return new Node(double.Parse(p[0]), double.Parse(p[1]));
                var edges = p.Skip(2)
                                .GetBigrams()
                                .ToList()
                                .ConvertAll(coord => new Node(double.Parse(coord.Item1), double.Parse(coord.Item2)))
                                .GetBigrams()
                                .ToList()
                                .ConvertAll(nodes => new Edge(nodes.Item1, nodes.Item2));
                return new Node(double.Parse(p[0]), double.Parse(p[1]));
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
                return (((Node)value).x + " " + ((Node)value).y + " " + string.Join(" ", ((Node)value).Edges));
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
