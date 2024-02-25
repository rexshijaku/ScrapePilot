using ScrapePilot.Attributes;
using ScrapePilot.Constants;
using ScrapePilot.Constants.InstructionType;

namespace ScrapePilot.Models.Instruction.Selenium
{
    [InstructionDetails(type: SeleniumInstuctionType.NAVIGATE_TO, driver: RecipeDriverType.SELENIUM)]
    public class NavTo
    {
        [InstructionArgumentDetails(attributeType: AttributeType.StoreKeyOrRawValue)]
        [InstructionArgumentClientDetails(name: "URL", description: "The URL of page which should be launched.")]
        public required string Url { get; set; }
    }
}
