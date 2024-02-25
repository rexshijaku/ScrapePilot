using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapePilot.Attributes
{
    [AttributeUsage(AttributeTargets.Property)] // Specify where this attribute can be applied
    public class LoopItemAttribute : Attribute
    {
        public string IDReplacable { get; set; }
        public LoopItemAttribute(string IDReplacable)
        {
            this.IDReplacable = IDReplacable;
        }
    }
}
