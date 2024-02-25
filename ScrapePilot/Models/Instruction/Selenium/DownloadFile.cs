﻿using ScrapePilot.Attributes;
using ScrapePilot.Constants;
using ScrapePilot.Constants.InstructionType;

namespace ScrapePilot.Models.Instruction.Selenium
{
    [InstructionDetails(type: SeleniumInstuctionType.WAIT_FILE_DOWNLOAD, driver: RecipeDriverType.SELENIUM)]
    public class DownloadFile
    {
        [InstructionArgumentDetails(attributeType: AttributeType.StoreKeyOrRawValue)]
        [InstructionArgumentClientDetails(name: "Source", description: "The URL source where the file is residing.")]
        public required string Src { get; set; }
    }
}