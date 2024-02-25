using ScrapePilot.Attributes;
using ScrapePilot.Constants;
using ScrapePilot.Constants.InstructionType;

namespace ScrapePilot.Models.Instruction.Selenium
{
    [InstructionDetails(type: SeleniumInstuctionType.PERFORM_CLICK, driver: RecipeDriverType.SELENIUM)]
    public class PerformClick
    {
        [InstructionArgumentDetails(attributeType: AttributeType.RawValue)]
        [InstructionArgumentClientDetails(name: "Element XPath", description: "The XPath of Element which should be clicked.")]
        public required string On { get; set; }
    }
}
