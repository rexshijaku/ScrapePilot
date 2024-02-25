using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapePilot.Attributes
{
    [AttributeUsage(AttributeTargets.Class)] // Specify where this attribute can be applied

    public class InstructionDetails : Attribute
    {
        public string _type { get; }
        public string _driver { get; }
        public bool _storable { get; }
        public string? _storeDetails { get; }

        public InstructionDetails(string type, string driver, bool storable = false, string? storeDetails = null)
        {
            _type = type;
            _driver = driver;

            _storable = storable;
            _storeDetails = storeDetails;
        }
    }
}
