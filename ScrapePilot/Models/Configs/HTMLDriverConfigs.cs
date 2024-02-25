using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScrapePilot.Attributes;
using ScrapePilot.Constants;

namespace ScrapePilot.Models.Configs
{
    [ConfigAttr(driver: RecipeDriverType.HtmlAguilitiPack)]
    public class HTMLDriverConfigs
    {
        [InstructionArgumentDetails(attributeType: AttributeType.RawValue)] // TODO #asthisOne1 This is not Instruction Property And We need to consider wether it should be renamed to PropertyDetails only!
        [InstructionArgumentClientDetails(name: "Encoding Name", description: "The Encoding Name.")]
        public string? EncodingName { get; set; }
    }
}
