using Microsoft.AspNetCore.Components;
using System.Reflection;
using ScrapePilot.Attributes;
using ScrapePilot.Models.Recipe;
using ScrapePilot.Client.Helpers;

namespace ScrapePilot.Client.Pages
{
    public partial class Instruction : ComponentBase
    {
        // Params
        [Parameter]
        public int InstructionNo { get; set; }

        [Parameter]
        public string? SelectedDriver { get; set; }

        [Parameter]
        public ScrapePilot.Models.Recipe.Instruction TheIstruction { get; set; }

        [Parameter]
        public Action? StateAndResultJSONReload { get; set; }

        [Parameter]
        public Action<int> OnInstructionRemove { get; set; }

        [Parameter]
        public SmartValuePicker SmartValuePicker { get; set; }

        public Dictionary<string, string> instructions = new Dictionary<string, string>();

        protected override async Task OnInitializedAsync()
        {
            StateAndResultJSONReload?.Invoke();
        }

        protected override void OnParametersSet()
        {
            // This method is called whenever the component's parameters are set or changed
            // You can perform additional logic here when parameters change
            base.OnParametersSet();
           
            LoadInstructions();
        }

        protected void InstructionOptionChanged(ChangeEventArgs e)
        {
            LoadInstructions();
            TheIstruction.type = e.Value.ToString();

            if(string.IsNullOrEmpty(TheIstruction.type))
            {
                return;
            }

            // instantiate instruction arguments!

            Type? theInstructionType = ReflectionHelper.GetInstructionType(TheIstruction.type);

            TheIstruction.arguments = Activator.CreateInstance(theInstructionType);

            foreach (PropertyInfo property in theInstructionType.GetProperties())
            {
                bool hasAttribute = Attribute.IsDefined(property, typeof(InstructionArgumentDetails));
                if (hasAttribute)
                {
                    object[] attributes = property.GetCustomAttributes(typeof(InstructionArgumentDetails), true);
                    if (((InstructionArgumentDetails)attributes[0])._attributeType == AttributeType.Composite)
                    {
                        property.SetValue(TheIstruction.arguments, Activator.CreateInstance(property.PropertyType));
                    }
                }
            }

            StateAndResultJSONReload?.Invoke();
        }

        protected void LoadInstructions()
        {
            instructions?.Clear();

            // get all by driver
            List<Type>? theInstructionlist = ReflectionHelper.GetInstructionTypesByDriver(SelectedDriver);

            if (theInstructionlist != null)
            {
                foreach (var itm in theInstructionlist)
                {
                    if (itm != null)
                    {
                        instructions[itm.Name]= itm.GetCustomAttribute<InstructionDetails>()._type;
                    }
                }
            }
        }

        #region Store
        protected void HandleStoreOptionChange(ChangeEventArgs e)
        {
            if ((bool)e.Value)
            {
                TheIstruction.store = new  ScrapePilot.Models.Recipe.Store() { name = "" };
            }
            else
            {
                TheIstruction.store = null;
            }
        }

        protected bool StoreIsChecked()
        {
            return TheIstruction.store != null;
        }

        protected string StoreError { get; set; } = string.Empty;

        protected void OnStoreKeyCreate(ChangeEventArgs e)
        {
            string? value = e.Value?.ToString(); 

            if(!String.IsNullOrEmpty(value) && !value.StartsWith("#"))
            {
                StoreError = "It must start with hash(#)!";
                return;
            }

            if (Utility.GetTheStore().Contains(value ?? ""))
            {
                StoreError = "This is in use!";
                return;
            }

            StoreError = string.Empty;

            if (TheIstruction.store != null)
            {
                TheIstruction.store.name = value;
                StateAndResultJSONReload?.Invoke();
            }
        }
        #endregion

        protected void RemoveThisInstruction()
        {
             // Call the parent function and pass the parameter
             OnInstructionRemove?.Invoke(InstructionNo - 1);
        }
    }
}
