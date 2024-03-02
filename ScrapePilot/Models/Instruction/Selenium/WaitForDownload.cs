using ScrapePilot.Attributes;
using ScrapePilot.Constants;
using ScrapePilot.Constants.InstructionType;

namespace ScrapePilot.Models.Instruction.Selenium
{
    [InstructionDetails(type: SeleniumInstuctionType.WAIT_FILE_DOWNLOAD, driver: RecipeDriverType.SELENIUM)]
    public class WaitForDownload
    {
        [InstructionArgumentDetails(attributeType: AttributeType.StoreKeyOrRawValue)]
        [InstructionArgumentClientDetails(name: "Source", description: "The File Name (with extension) which will be downloaded to the Output Directory.")]
        public required string Src { get; set; }
    }
}
