using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScrapePilot.Attributes;
using ScrapePilot.Constants.InstructionType;
using ScrapePilot.Constants;

namespace ScrapePilot.Models.Instruction.Selenium
{
    [InstructionDetails(type: SeleniumInstuctionType.EXTRACT_MULTI_PAGE_TABLE_DATA, driver: RecipeDriverType.SELENIUM,
        storable: true, storeDetails: "The Extracted Multi Page Value Will be Stored as 2D.")]
    public class ExtractMultiPageTableData
    {
        [InstructionArgumentDetails(attributeType: AttributeType.RawValue)]
        [InstructionArgumentClientDetails(name: "Page Changer", description: "Button which changes the page when clicked.")]
        public required string PageChanger { get; set; }

        [InstructionArgumentDetails(attributeType: AttributeType.RawValue)]
        [InstructionArgumentClientDetails(name: "Watch Value", description: "The Element whos value is changed when Page is changed.")]
        public required string ChangeIsDetectedWhenThisElementsValueChanged { get; set; } // todo can be more advanced/general
        
        [InstructionArgumentDetails(attributeType: AttributeType.RawValue)]
        [InstructionArgumentClientDetails(name: "Stop when Page Changer is disabled", description: "If in the last page the Page Changer is getting diabled then this should be checked.")]
        public required bool StopWhenElemIsDisabled { get; set; }   // todo more advanced/general?
        
        [InstructionArgumentDetails(attributeType: AttributeType.Composite)]
        [InstructionArgumentClientDetails(name: "Table XPaths", description: "The XPath of how to treat the Tables.")]
        public required Html.ExtractTableData TableDataXPath { get; set; }

        //(//*[contains(@class, 'ui-paginator-next')])[1]
        //(//span[@class=\"ui-paginator-current\"])[1]
    }
}
