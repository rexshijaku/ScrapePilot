using System.Reflection;
using ScrapePilot.Client.Pages;

namespace ScrapePilot.Client.Helpers
{
    // This Class provides various functions to be used across the project
    public class Utility
    {
        public static List<string> GetTheStore()
        {
            List<string> store = new List<string>();
            if (Home.RecipeMain != null)
            {
                foreach (var recipe in Home.RecipeMain.recipes)
                {
                    foreach (var ins in recipe.instructions)
                    {
                        if (ins.store != null)
                        {
                            store.Add(ins.store.name);
                        }
                    }
                }
            }
            return store;
        }
    }
}
