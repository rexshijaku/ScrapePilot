using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScrapePilot.Attributes;

namespace ScrapePilot
{
    public class AppConfiguration
    {
        [AppVariable(variableNameInRecipe: "#output_path")]
        public string? OutputPath { get; set; }

        [AppVariable(variableNameInRecipe: "#verbose")]
        public bool Verbose { get; set; }
    }
}
