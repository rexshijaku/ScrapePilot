using Microsoft.AspNetCore.Components;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using ScrapePilot.Models.Recipe;
using ScrapePilot.Client.Helpers;

namespace ScrapePilot.Client.Pages
{
    public partial class PropHandler : ComponentBase
    {
        [Parameter]
        public dynamic TheClassInstance { get; set; }

        [Parameter]
        public PropertyInfo property { get; set; }

        [Parameter]
        public Action? StateAndResultJSONReload { get; set; }

        [Parameter]
        public SmartValuePicker SmartValuePicker { get; set; }

        protected override void OnParametersSet()
        {
            // This method is called whenever the component's parameters are set or changed
            // You can perform additional logic here when parameters change
            base.OnParametersSet();
        }

        public async Task SetAnyValue(dynamic instance, string propName, object? value)
        {
            ReflectionHelper.SetPropValue(instance,propName, value);  

            await Task.CompletedTask;

            StateAndResultJSONReload?.Invoke();
        }

        private void SetItem(ChangeEventArgs e, List<string>? refBucket, dynamic instance, string propName)
        {

            refBucket = ((string[])e.Value).ToList().Where(v => v.Length > 0).ToList();

            if(refBucket.Any())
            {
                SetAnyValue(instance, propName, refBucket.Distinct().ToList());
            }
            else
            {
                SetAnyValue(instance, propName, null);
            }
        }

        private async Task AddValue(List<string> list, string newVal, PropertyInfo propRef, dynamic instance)
        {
            Console.WriteLine("Adding a Value.");
            list.Add(newVal);
            propRef.SetValue(instance, list);
            await Task.CompletedTask;
            StateAndResultJSONReload?.Invoke();
            Console.WriteLine("Value Added.");
        }

        private async Task SetAValue(List<string> list, int index,string newVal, PropertyInfo propRef, dynamic instance)
        {
            Console.WriteLine("Adding a Value.");
            list[index] = newVal;
            propRef.SetValue(instance, list);
            await Task.CompletedTask;
            StateAndResultJSONReload?.Invoke();
            Console.WriteLine("Value Added.");
        }

        private async Task RemoveValue(List<string> list, int index, PropertyInfo propRef, dynamic instance)
        {
            Console.WriteLine("Removing a Value.");
            list.RemoveAt(index);
            propRef.SetValue(instance, list);
            await Task.CompletedTask;
            StateAndResultJSONReload?.Invoke();
            Console.WriteLine("Value Removed.");
        }

        private async Task AddItemToListTypeProperty(object obj, PropertyInfo property, Type itemType)
        {
            //PropertyInfo property = obj.GetType().GetProperty(propertyName);
            if (property != null && property.PropertyType.IsGenericType &&
            property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
            {
                MethodInfo addMethod = property.PropertyType.GetMethod("Add");
                if (addMethod != null)
                {
                    object newItem = Activator.CreateInstance(itemType);
                    addMethod.Invoke(property.GetValue(obj), new object[] { newItem });
                }
            }
            await Task.CompletedTask;
            StateAndResultJSONReload?.Invoke();
        }
    }
}