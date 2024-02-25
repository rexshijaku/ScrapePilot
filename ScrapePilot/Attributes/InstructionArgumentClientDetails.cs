using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapePilot.Attributes
{
    [AttributeUsage(AttributeTargets.Property)] // Specify where this attribute can be applied
    public class InstructionArgumentClientDetails : Attribute
    {
        /*
         * USING THIS ATTRIBUTE MEANS THE VALUE OF THAT PROPERTY WILL BE CHECKED IF EXISTS AS A KEY IN A CACHE OF COLLECTED RESULTS0
         */

        public string _name { get; }
        public string _description { get; }

        public InstructionArgumentClientDetails(string name, string description)
        {
            _name = name;
            _description = description;
        }
    }
}
