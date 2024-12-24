using Shared.Enums;
using System;
using System.ComponentModel;
using System.Reflection;

namespace Shared.Extentions
{
    public static class EnumExt
    {
        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    DescriptionAttribute attr =
                           Attribute.GetCustomAttribute(field,
                             typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            return null!;
        }

        public static string GetName(this AuthRoles value)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])value
                   .GetType()
                   .GetField(value.ToString())
                   .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }

        public static string GetName(this AzureFunctions value)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])value
                   .GetType()
                   .GetField(value.ToString())
                   .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }
    }
}
