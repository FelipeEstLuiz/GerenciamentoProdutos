using Application.Domain.Attributes;
using System.Reflection;

namespace Application.Domain.Extension;

public static class EnumExtensions
{
    public static string GetEnumName<T>(this T enumValue) where T : Enum
    {
        FieldInfo? fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

        return fieldInfo?.GetCustomAttributes(typeof(EnumNameAttribute), false) is EnumNameAttribute[] attributes && attributes.Length > 0
            ? attributes[0].Name
            : enumValue.ToString();
    }
}
