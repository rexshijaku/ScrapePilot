using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapePilot.Attributes
{
    [AttributeUsage(AttributeTargets.Property)] // Specify where this attribute can be applied
    public class UsesValuesOfAttr : Attribute
    {
        public string _of { get; }
        public UsesValuesOfAttr(string of)
        {
            _of = of;
        }
    }
}
