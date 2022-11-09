using System;
using System.Linq;

namespace ArsuLeo.CS.Utils.Service.CSharp
{
    public static class ClassAttributeUtil
    {
        public static string? GetAttributeStringValue<TAttribute>(
            this Type type,
            Func<TAttribute, string> valueSelector)
            where TAttribute : Attribute
        {
            if (type.GetCustomAttributes(
                typeof(TAttribute), true
            ).FirstOrDefault() is TAttribute att)
            {
                return valueSelector(att);
            }
            return default;
        }
    }
}
