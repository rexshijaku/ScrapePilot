using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ScrapePilot.Client.Helpers
{
    // Provides functions to Get The Value Of Different Types when User is Changing some Dynamic Input type 
    public class ValueHelper
    {
        public static int? GetIntOrNull(object? inputValue)
        {
            if (inputValue == null) return null;

            if (int.TryParse(inputValue.ToString(), out int integerValue))
            {
                return integerValue;
            }
            else
            {
                return null;
            }
        }

        public static double? GetDoubleOrNull(object? inputValue)
        {
            if (inputValue == null) return null;

            if (double.TryParse(inputValue.ToString(), out double numericValue))
            {
                return numericValue;
            }
            else
            {
                return null;
            }
        }

        public static object? GetDefaultValue(PropertyInfo propertyInfo)
        {
            object[] attributes = propertyInfo.GetCustomAttributes(typeof(DefaultValueAttribute), false);

            if (attributes.Length > 0)
            {
                DefaultValueAttribute defaultValueAttribute = (DefaultValueAttribute)attributes[0];
                return defaultValueAttribute.Value;
            }
            else
            {
                // If DefaultValueAttribute is not present, return the default value for the property type
                return Activator.CreateInstance(propertyInfo.PropertyType);
            }
        }

        public static bool PropIsRequired(PropertyInfo propertyInfo)
        {
            return Attribute.IsDefined(propertyInfo, typeof(RequiredMemberAttribute));
        }

    }
}
