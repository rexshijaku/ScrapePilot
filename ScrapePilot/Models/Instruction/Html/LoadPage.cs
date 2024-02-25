using ScrapePilot.Attributes;
using ScrapePilot.Constants;
using ScrapePilot.Constants.InstructionType;

namespace ScrapePilot.Models.Instruction.Html
{
    [InstructionDetails(type: HtmlInstuctionType.LOAD_PAGE, driver: RecipeDriverType.HtmlAguilitiPack)]
    public class LoadPage
    {
        [InstructionArgumentDetails(attributeType: AttributeType.StoreKeyOrRawValue)]
        [InstructionArgumentClientDetails(name: "URL", description: "The Page URL to Load.")]
        public required string Url { get; set; }
    }
}
