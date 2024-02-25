
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ScrapePilot.Attributes;

namespace ScrapePilot.Models.Recipe
{
    public class Use
    {
        [UsesValuesOfAttr(of:(nameof(Constants.RecipeDriverType)))]
        public required string driver { get; set; }

        public dynamic? configs { get; set; }

        public void SayUsingDriver()
        {
            App._consolePrinter.Print($"Using Driver: {this.driver}...");
        }

        public T? GetConfigs<T>()
        {
            if(configs is null)
            {
                return default(T);
            }

            return JsonSerializer.Deserialize<T>(configs.ToString());
        }
    }
}
