using Microsoft.AspNetCore.Components;
using System.Reflection;
using ScrapePilot.Constants;
using ScrapePilot.Models.Recipe;
using ScrapePilot.Client.Helpers;
using ScrapePilot.Client.Models;

namespace ScrapePilot.Client.Pages
{
    public partial class Recipe : ComponentBase
    {
        // Params
        [Parameter]
        public ScrapePilot.Models.Recipe.Recipe ThisRecipe { get; set; }

        [Parameter]
        public int RecipeNo { get; set; }

        [Parameter]
        public Action? StateAndResultJSONReload { get; set; }

        [Parameter]
        public Action<int> OnRecipeRemoved { get; set; }
        [Parameter]
        public SmartValuePicker SmartValuePicker { get; set; }

        // Defaults 
        string SelectedInstructionOption { get; set; } = String.Empty;
        string DefaultSelectedDriver { get; set; } = RecipeDriverType.SELENIUM;

        // Global Lists
        List<string> DriverTypes = new List<string>();

        protected override async Task OnInitializedAsync()
        {
            LoadRecipeConfigs();

            ThisRecipe.use.driver = DefaultSelectedDriver;
            DriverTypes = RecipeDriverType.GetAll();

            StateAndResultJSONReload?.Invoke();
        }

        private void DriverTypeChanged(ChangeEventArgs e)
        {
            SelectedInstructionOption = string.Empty;
            DefaultSelectedDriver = e.Value.ToString();

            ThisRecipe.use.driver = DefaultSelectedDriver ?? String.Empty;
            ThisRecipe.instructions.Clear();

            LoadRecipeConfigs();
            StateAndResultJSONReload?.Invoke();
        }

        private void AddAnInstruction()
        {
            ThisRecipe.instructions.Add(new ScrapePilot.Models.Recipe.Instruction()
            {
                type = string.Empty,
                store = null,
                arguments = null
            });
        }

        private void RemoveAnInstruction(int index)
        {
            ThisRecipe.instructions.RemoveAt(index);
            StateAndResultJSONReload?.Invoke();
        }
       
        private void LoadRecipeConfigs()
        {
            Type? theConfigType = ReflectionHelper.GetDriverClass(DefaultSelectedDriver);
            ThisRecipe.use.configs = theConfigType == null ? null : Activator.CreateInstance(theConfigType);
        }
       
        private void RemoveThisRecipe()
        {
            OnRecipeRemoved?.Invoke(RecipeNo - 1);
        }
    }
}
