using System.ComponentModel;
using System.Globalization;

namespace Dfe.CaseAggregationService.Application.Common.Extensions
{
    public static class EnumExtensions
    {
        public static string ToDescription<T>(this T? source)
        {
            if (source == null) return string.Empty;

            var fi = source.GetType().GetField(source.ToString());

            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute), false);

            return attributes != null && attributes.Length > 0
                ? attributes[0].Description
                : source.ToString();
        }
    }

}
