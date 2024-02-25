using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScrapePilot.Attributes;
using ScrapePilot.Constants;

namespace ScrapePilot.Models.Configs
{
    [ConfigAttr(driver: RecipeDriverType.SELENIUM)]
    public class SeleniumDriverConfigs
    {
        [InstructionArgumentDetails(attributeType: AttributeType.RawValue)] // TODO #asthisOne1
        [InstructionArgumentClientDetails(name: "Headless", description: "Should the Browser be opened when the Selenium Process is Running!")]
        public bool? Headless { get; set; } = true;

    }
}
