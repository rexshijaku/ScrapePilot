using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapePilot.Attributes
{
    [AttributeUsage(AttributeTargets.Property)] // Specify where this attribute can be applied
    public class InstructionArgumentDetails : Attribute
    {

        /*
         * USING THIS ATTRIBUTE MEANS THE VALUE OF THAT PROPERTY WILL BE CHECKED IF EXISTS AS A KEY IN A CACHE OF COLLECTED RESULTS0
         */

        public AttributeType _attributeType { get; }
        public Type? _listSource { get; }
       
        public InstructionArgumentDetails(AttributeType attributeType, Type? listSource = null)
        {
            _attributeType = attributeType;
            _listSource = listSource;
        }
    }

    public enum AttributeType
    {
        StoreKey = 1,
        RawValue = 2,
        StoreKeyOrRawValue = 3,
        ListItemMulti = 4, 
        ListItemSingle = 5,
        Composite= 6,
        ListAsRawOrFromStore= 7,
        CompositeList = 8

    }
}
