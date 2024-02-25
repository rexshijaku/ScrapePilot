using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapePilot.Attributes
{
    [AttributeUsage(AttributeTargets.Property)] // Specify where this attribute can be applied
    public class AppVariable : Attribute
    {
        public string VariableNameInRecipe { get; }
        public AppVariable(string variableNameInRecipe)
        {
            VariableNameInRecipe = variableNameInRecipe;
        }
    }
}
