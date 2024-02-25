using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapePilot.Attributes
{
    [AttributeUsage(AttributeTargets.Class)] // Specify where this attribute can be applied
    public class ConfigAttr : Attribute
    {
        public string _driver { get; }
        public ConfigAttr(string driver)
        {
            _driver = driver;
        }
    }
}
