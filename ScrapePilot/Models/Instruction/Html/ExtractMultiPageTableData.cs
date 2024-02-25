using System.ComponentModel;
using ScrapePilot.Attributes;
using ScrapePilot.Constants;
using ScrapePilot.Constants.InstructionType;

namespace ScrapePilot.Models.Instruction.Html
{
    [InstructionDetails(type: HtmlInstuctionType.EXTRACT_MULTI_PAGE_TABLE_DATA, driver: RecipeDriverType.HtmlAguilitiPack,
        storable: true, storeDetails: "Stores the Data of Each Extracted Page in a 2D List.")]
    public class ExtractMultiPageTableData
    {

        [InstructionArgumentDetails(attributeType: AttributeType.RawValue)]
        [InstructionArgumentClientDetails(name: "The Loopable URL", description: "A paginatable URL which contains a part it leads to a different page of the multi paged html table. It must contain @@OFFSET@@ part!")]
        public required string LoopableUrl { get; set; }

        [InstructionArgumentDetails(attributeType: AttributeType.RawValue)]
        [InstructionArgumentClientDetails(name: "The Row XPath", description: "The XPath which extracts all rows of the table.")]
        public required string RowXPath { get; set; }

        [InstructionArgumentDetails(attributeType: AttributeType.RawValue)]
        [InstructionArgumentClientDetails(name: "Skip Rows", description: "How many rows should be skipped on each page.")]
        public int SkipRows { get; set; }

        [InstructionArgumentDetails(attributeType: AttributeType.RawValue)]
        [InstructionArgumentClientDetails(name: "Row Limit", description: "How many rows to be fetched on each page.")]
        public int RowLimit { get; set; }

        [InstructionArgumentDetails(attributeType: AttributeType.RawValue)]
        [InstructionArgumentClientDetails(name: "Sort Column", description: "The column index which should be used to sort the complete table.")]
        public int SortColumn { get; set; }

        [InstructionArgumentDetails(attributeType: AttributeType.RawValue)]
        [InstructionArgumentClientDetails(name: "Link Cell XPath", description: "TODO")]
        public string? LinkCellPath { get; set; }

        [InstructionArgumentDetails(attributeType: AttributeType.RawValue)]
        [InstructionArgumentClientDetails(name: "Pagination Start", description: "Pagination first page of the Loopable URL.")]
        public int PaggingOffset { get; set; }

        [InstructionArgumentDetails(attributeType: AttributeType.RawValue)]
        [InstructionArgumentClientDetails(name: "Pagination Step", description: "Pagination step of the Loopable URL.")]
        public int IncreaseOffset { get; set; }

        [InstructionArgumentDetails(attributeType: AttributeType.RawValue)]
        [InstructionArgumentClientDetails(name: "Min page", description: "The first page.")]
        [DefaultValue(int.MinValue)]
        public int PageMinLimit { get; set; } = int.MinValue;

        [InstructionArgumentDetails(attributeType: AttributeType.RawValue)]
        [InstructionArgumentClientDetails(name: "Max page", description: "The last page.")]
        [DefaultValue(int.MaxValue)]
        public int PageMaxLimit { get; set; } = int.MaxValue;

        /*
           * An item Example used in IATCC Source:
           * {
              "type": "save_txt_file",
              "arguments": {
                "Path": "c:\\tmp\\filename.txt",
                "UseContentOf": "loopResult"
              }
            }
        */
    }
}
