using ScrapePilot.Attributes;
using ScrapePilot.Constants.InstructionType;
using ScrapePilot.Constants;

namespace ScrapePilot.Models.Instruction.Html
{
    [InstructionDetails(type: HtmlInstuctionType.EXTRACT_TABLE_XPATH_LIST, driver: RecipeDriverType.HtmlAguilitiPack,
        storable: true, storeDetails: "Stores the XPath List")]
    public class ExtractTableXPathList
    {
        [InstructionArgumentDetails(attributeType: AttributeType.RawValue)]
        [InstructionArgumentClientDetails(name: "The Source URL", description: "The Source URL where the targe table will be fetched.")]
        public required string From { get; set; }

        [InstructionArgumentDetails(attributeType: AttributeType.RawValue)]
        [InstructionArgumentClientDetails(name: "Table XPath", description: "The XPath which fetches the Table.")]
        public required string TableXPath { get; set; } = "//table";
    }
}
