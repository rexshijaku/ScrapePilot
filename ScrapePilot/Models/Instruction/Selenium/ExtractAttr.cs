using ScrapePilot.Attributes;
using ScrapePilot.Constants;
using ScrapePilot.Constants.InstructionType;

namespace ScrapePilot.Models.Instruction.Selenium
{
    [InstructionDetails(type: SeleniumInstuctionType.EXTRACT_ATTR, driver: RecipeDriverType.SELENIUM,
        storable: true, storeDetails: "The Extracted Attribute Value Will be Stored.")]
    public class ExtractAttr
    {
        [InstructionArgumentDetails(attributeType: AttributeType.RawValue)]
        [InstructionArgumentClientDetails(name: "Atrribute", description: "The attribute from which the value should be extracted.")]
        public required string Attr { get; set; }

        [InstructionArgumentDetails(attributeType: AttributeType.RawValue)]
        [InstructionArgumentClientDetails(name: "Element", description: "The element which contains the attribute.")]
        public required string From { get; set; }

        [InstructionArgumentDetails(attributeType: AttributeType.ListItemMulti, listSource: typeof(ConstraintType))]
        [InstructionArgumentClientDetails(name: "Constraints", description: "The contraints that the extracted value should respect.")]
        public List<string>? Contraints { get; set; }
    }
}
