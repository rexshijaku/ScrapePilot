using System.ComponentModel;
using System.Reflection;
using ScrapePilot.Attributes;
using ScrapePilot.Client.Models;

namespace ScrapePilot.Client.Helpers
{
    // This Class provides functions for abstracting the reflection class
    public class ReflectionHelper
    {
        static Assembly? projectAAssembly;

        static ReflectionHelper()
        {
            projectAAssembly = Assembly.Load(Constants.LIBRARY_NAME);
        }

        /*
         * Get Fields and Properties of a Given Type
         */
        public static List<MemberModel> GetTypeMembers(Type theType)
        {
            List<MemberModel> allMembers = new List<MemberModel>();

            PropertyInfo[] properties = theType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            
            foreach (PropertyInfo propertyInfo in properties)
            {
                // Get the default value of the property
                object? defaultValue = GetDefaultValueForProperty(propertyInfo);

                allMembers.Add(new MemberModel()
                {
                    Name = propertyInfo.Name,
                    Type = propertyInfo.PropertyType,
                    DefaultValue = defaultValue
                });
            }

            FieldInfo[]? fields = theType.GetFields(); // such as  constants
            foreach (FieldInfo? fieldInfo in fields)
            {
                // Get the default value of the constant
                object? defaultValue = fieldInfo.GetValue(null); // Constants are static, so pass null for instance

                allMembers.Add(new MemberModel()
                {
                    Name = fieldInfo.Name,
                    Type = fieldInfo.FieldType,
                    DefaultValue = defaultValue
                });
            }

            return allMembers;
        }

        public static Tuple<string, string> GetInstructionArgumentsNameDescription(PropertyInfo property)
        {
            Attribute? propClientDetails = property.GetCustomAttribute(typeof(InstructionArgumentClientDetails));
            if (propClientDetails is null)
            {
                return new Tuple<string, string>("??", "??");
            }

            InstructionArgumentClientDetails argDetails = (InstructionArgumentClientDetails)propClientDetails;
            if (argDetails is null)
            {
                return new Tuple<string, string>(string.Empty, string.Empty);
            }

            return new Tuple<string, string>(argDetails._name, argDetails._description);
        }

        private static object? GetDefaultValueForProperty(PropertyInfo property)
        {
            Attribute? defaultAttr = property.GetCustomAttribute(typeof(DefaultValueAttribute));
            if (defaultAttr != null)
            {
                return (defaultAttr as DefaultValueAttribute).Value;
            }

            Type? propertyType = property.PropertyType;
            propertyType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;
            return propertyType.IsValueType ? Activator.CreateInstance(propertyType) : null;
        }

        public static List<Type>? GetInstructionTypesByDriver(string driver)
        {
            return projectAAssembly?.GetTypes()
                      .Where(type => type.Namespace != null && type.Namespace.StartsWith(Constants.INSTRUCTION_TYPES_PATH)
                        && type.GetCustomAttribute<InstructionDetails>()?._driver == driver)
                      .ToList();
        }

        public static Tuple<bool, string?> GetStoreItem(Type theType)
        {
            InstructionDetails? tm = theType.GetCustomAttribute<InstructionDetails>();

            if (tm == null)
            {
                return new Tuple<bool, string?>(false, null); 
            }

            return new Tuple<bool, string?>(tm._storable, tm._storeDetails);
        }

        public static Type? GetInstructionType(string instructionType)
        {
            return projectAAssembly?.GetTypes()
                       .Where(type => type.Namespace != null 
                        && type.Namespace.StartsWith(Constants.INSTRUCTION_TYPES_PATH)
                         && type.GetCustomAttribute<InstructionDetails>()?._type == instructionType)
                       .FirstOrDefault();
        }

        public static Type? GetDriverClass(string driver) // Get the Class Config for the Given Driver
        {
            return projectAAssembly?.GetTypes()
                .Where(type => type.Namespace == Constants.CONFIGS_PATH
                  && type.GetCustomAttribute<ConfigAttr>()?._driver == driver)
                .FirstOrDefault();
        }

        public static void SetPropValue(dynamic instance, string propName, object? value)
        {
            PropertyInfo? propertyInfo = instance.GetType().GetProperty(propName);

            // Check if the property exists
            if (propertyInfo != null)
            {
                if (value == null)
                {
                    value = ValueHelper.GetDefaultValue(propertyInfo);
                }
                // when you delete the last char the string remains as empty, but if is not required it should be null
                else if (
                    value != null &&
                    propertyInfo.PropertyType == typeof(string) &&
                    value.ToString() == string.Empty &&
                    !ValueHelper.PropIsRequired(propertyInfo))
                {
                    value = null;
                }

                // Set the value of the property
                propertyInfo.SetValue(instance, value);
            }
            else
            {
                // Handle the case where the property is not found
                Console.WriteLine("Some Error happened while SetAnyValue");
            }
        }

        public static string GetFieldDescription(FieldInfo field)
        {
            Attribute? defaultAttr = field.GetCustomAttribute(typeof(DescriptionAttribute));
            if (defaultAttr != null)
            {
                return (defaultAttr as DescriptionAttribute).Description;
            }

            return string.Empty;
        }

        public static Dictionary<string, string> GetListSource(Type type)
        {
            Dictionary<string, string> list = new Dictionary<string, string>();

            FieldInfo[] fields = type.GetFields(BindingFlags.Public
                                              | BindingFlags.Static 
                                              | BindingFlags.FlattenHierarchy);

            for (int i = 0; i < fields.Length; i++)
            {
                var val = (string)(fields[i].GetValue(null));
                list.Add(val, ReflectionHelper.GetFieldDescription(fields[i]));
            }

            return list;
        }

        public static string[] GetListSourceAsArray(Type type)
        {
            FieldInfo[] fields = type.GetFields(BindingFlags.Public
                                              | BindingFlags.Static
                                              | BindingFlags.FlattenHierarchy);

            string[] values = new string[fields.Length];

            for (int i = 0; i < fields.Length; i++)
            {
                values[i] = (string)(fields[i].GetValue(null) ?? string.Empty);
            }

            return values;
        }
    }
}
