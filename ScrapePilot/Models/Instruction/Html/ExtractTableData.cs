using ScrapePilot.Attributes;
using ScrapePilot.Constants;
using ScrapePilot.Constants.InstructionType;

namespace ScrapePilot.Models.Instruction.Html
{
    [InstructionDetails(type: HtmlInstuctionType.EXTRACT_TABLE_DATA, driver: RecipeDriverType.HtmlAguilitiPack,
        storable: true, storeDetails: "Stores the Extracted Table Data in a 2D List.")]
    public class ExtractTableData
    {
        [InstructionArgumentDetails(attributeType: AttributeType.RawValue)]
        [InstructionArgumentClientDetails(name: "The Row XPath", description: "The XPath which extracts all rows of the table.")]
        public required string RowXPath { get; set; }

        [InstructionArgumentDetails(attributeType: AttributeType.RawValue)]
        [InstructionArgumentClientDetails(name: "Skip Rows", description: "How many rows should be skipped/not fetched.")]
        public int SkipRows { get; set; }

        [InstructionArgumentDetails(attributeType: AttributeType.RawValue)]
        [InstructionArgumentClientDetails(name: "Sort Column", description: "The column index which serves to sort the table.")]
        public int SortColumn { get; set; }

        [InstructionArgumentDetails(attributeType: AttributeType.RawValue)]
        [InstructionArgumentClientDetails(name: "Row Limit", description: "How many rows should be fetched at max.")]
        public int RowLimit { get; set; }

        [InstructionArgumentDetails(attributeType: AttributeType.RawValue)]
        [InstructionArgumentClientDetails(name: "Link Cell XPath", description: "TODO")]
        public string? LinkCellPath { get; set; }
    }
}
