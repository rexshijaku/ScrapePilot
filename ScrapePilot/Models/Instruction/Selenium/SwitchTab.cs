using ScrapePilot.Attributes;
using ScrapePilot.Constants;
using ScrapePilot.Constants.InstructionType;
using ScrapePilot.Constants.InstructionValue.Selenium;

namespace ScrapePilot.Models.Instruction.Selenium
{
    [InstructionDetails(type: SeleniumInstuctionType.SWITCH_TAB, driver: RecipeDriverType.SELENIUM)]
    public class SwitchTab
    {
        [InstructionArgumentDetails(attributeType: AttributeType.ListItemSingle, listSource:typeof(SwitchTabVal))]
        [InstructionArgumentClientDetails(name: "Tab", description: "The Tab that should come to focus.")]
        public required string To { get; set; }
    }
}
