using System.ComponentModel;
using System.Reflection;

namespace ConvenienceStoreApi.Application.Common.Utils;

public static class EnumUtil
{
    public static string GetEnumDescription<T>(T value) where T : Enum
    {
        FieldInfo field = value.GetType().GetField(value.ToString());

        if (field == null)
            return null;

        DescriptionAttribute attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));

        return attribute == null ? value.ToString() : attribute.Description;
    }

    public static T GetEnumValueFromDescription<T>(string description) where T : Enum
    {
        foreach (var field in typeof(T).GetFields())
        {
            var attribute = field.GetCustomAttribute<DescriptionAttribute>();
            if (attribute != null && attribute.Description == description)
            {
                return (T)field.GetValue(null);
            }
        }

        throw new ArgumentException($"No se encontró ningún valor para la descripción: {description}");
    }
}