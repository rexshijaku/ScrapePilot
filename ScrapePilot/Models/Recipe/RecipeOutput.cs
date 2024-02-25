using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScrapePilot.Attributes;

namespace ScrapePilot.Models.Recipe
{
    public class RecipeOutput
    {
        /*
        * List of string fragments, a fragment can also be a store key
        */
        public required List<string> value { get; set; }

        [UsesValuesOfAttr(of: (nameof(Constants.OutputType)))]
        public required string type { get; set; }
    }
}
