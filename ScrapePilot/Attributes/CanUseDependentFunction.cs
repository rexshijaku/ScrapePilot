using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapePilot.Attributes
{
    [AttributeUsage(AttributeTargets.Property)] // Specify where this attribute can be applied
    public class CanUseDependentFunction : Attribute
    {
        public string _referToField { get; set; }

        public CanUseDependentFunction(string referToField)
        {
            _referToField = referToField;
        }
    }
}
