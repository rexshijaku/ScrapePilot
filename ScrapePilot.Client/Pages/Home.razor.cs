using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text.Json;
using ScrapePilot.Models.Recipe;
using ScrapePilot.Client.Helpers;
using ScrapePilot.Client.Models;

namespace ScrapePilot.Client.Pages
{
    public partial class Home : ComponentBase
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public static RecipeMain RecipeMain { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        List<MemberModel> recipeOutputTypes = new List<MemberModel>();

        Dictionary<int, bool> selectedIndexes = new Dictionary<int, bool>();

        [Inject] IJSRuntime? JSRuntime { get; set; }

        public SmartValuePicker? SmartValuePickerVar { get; set; }

        protected override void OnInitialized()
        {
            LoadRecipeOutputTypes();
            Console.WriteLine("Initiating a Recipe...");
            RecipeMain = new RecipeMain()
            {
                recipes = new List<ScrapePilot.Models.Recipe.Recipe>()
                {

                },
                output = new RecipeOutput()
                {
                    type = recipeOutputTypes.First().Name ?? string.Empty,
                    value = new List<string>()
                }
            };
            Console.WriteLine("Recipe initiated.");
            StateAndResultJSONReload();
        }

        #region Recipe
        private void AddRecipe()
        {
            Console.WriteLine("Adding a new Recipe.");

            RecipeMain?.recipes.Add(new ScrapePilot.Models.Recipe.Recipe()
            {
                use = new Use()
                {
                    driver = string.Empty,
                    configs = null
                },
                instructions = new List<ScrapePilot.Models.Recipe.Instruction>()
                {

                }
            });

            Console.WriteLine($"New Recipe Added. Now Total Recipe Count: {RecipeMain.recipes.Count}");
        }

        private void RemoveRecipe(int index)
        {
            Console.WriteLine($"Removing the Recipe {index + 1}");

            RecipeMain.recipes.RemoveAt(index);
            StateAndResultJSONReload();

            Console.WriteLine($"The Recipe {index + 1} Removed.");
        }
        #endregion

        #region Store
        private void UpdateListStoreList(ChangeEventArgs e, int index)
        {
            RecipeMain.output.value[index] = e.Value.ToString();
            StateAndResultJSONReload();
        }
        #endregion

        #region Output
        private void LoadRecipeOutputTypes()
        {
            Console.WriteLine("Loading Recipe Output Types...");

            recipeOutputTypes.Clear();
            recipeOutputTypes = ReflectionHelper.GetTypeMembers(typeof(ScrapePilot.Constants.OutputType));

            Console.Write("Recipe Output Types Loaded!");
        }

        private void AddOutputValueFragment()
        {
            Console.WriteLine("Adding an Output Part Input Item.");

            RecipeMain.output.value.Add(String.Empty);
            StateAndResultJSONReload();

            Console.WriteLine("Output Part Input Item Added.");
        }

        private void OutputTypeChanged(ChangeEventArgs e)
        {
            Console.WriteLine("Changning the Output Type of Recipe.");

            var val = e.Value;
            if (val != null)
            {
                RecipeMain.output.type = val.ToString();
                StateAndResultJSONReload();
            }

            Console.WriteLine("Output Type of the Recipe changed.");
        }
        #endregion

        #region JSON Result
        private string jsonString = string.Empty; // This is where you store your JSON string
        private string? formattedJson { get; set; } // Format the JSON string

        private void StateAndResultJSONReload()
        {
            Console.WriteLine("Updating State and JSON Result...");
            StateHasChanged();

            jsonString = JsonSerializer.Serialize(RecipeMain,
                new JsonSerializerOptions { WriteIndented = true });

            formattedJson = JsonFormatter.Format(jsonString);

            Console.WriteLine("State and JSON Result was updated!");
        }

        private async Task CopyTheResult()
        {
            Console.WriteLine("****** --------");

            Console.WriteLine("Running the Script to copying the result...");

            await JSRuntime.InvokeVoidAsync("copyText");

            Console.WriteLine("Script to copy the result RAN!");
        }
        #endregion
  
    }
}
