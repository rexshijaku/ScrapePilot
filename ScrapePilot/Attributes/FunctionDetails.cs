using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapePilot.Attributes
{
    [AttributeUsage(AttributeTargets.Method)] // Specify where this attribute can be applied
    public class FunctionDetails : Attribute
    {
        public string _name { get; }
        public FunctionDetails(string name)
        {
            _name = name;
        }
    }
}
