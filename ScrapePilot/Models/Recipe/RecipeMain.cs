using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScrapePilot.Constants;
using ScrapePilot.Helpers;
using ScrapePilot.Models.Instruction;

namespace ScrapePilot.Models.Recipe
{
    public class RecipeMain
    {
        public required List<Recipe> recipes { get; set; }
        public required RecipeOutput output { get; set; }

        public Dictionary<string, string>? configs { get; set; }

        public string getOutputValue()
        {
            return FileHelper.GetFragmented(output.value);
        }
    }
}
