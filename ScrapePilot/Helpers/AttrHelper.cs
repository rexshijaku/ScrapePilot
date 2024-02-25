using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ScrapePilot.Helpers
{
    public class AttrHelper
    {
         public static T GetCustomAttribute<T>(Type type, string propertyName) where T : Attribute
         {
            PropertyInfo propertyInfo = type.GetProperty(propertyName);

            if (propertyInfo != null)
            {
                T attribute = (T)Attribute.GetCustomAttribute(propertyInfo, typeof(T));

                return attribute;
            }

            return null;
         }
    }
}
