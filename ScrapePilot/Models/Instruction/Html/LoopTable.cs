using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScrapePilot.Attributes;
using ScrapePilot.Constants;
using ScrapePilot.Constants.InstructionType;

namespace ScrapePilot.Models.Instruction.Html
{
    [InstructionDetails(type: HtmlInstuctionType.LOOP_A_TABLE, driver: RecipeDriverType.HtmlAguilitiPack)]
    public class LoopTable
    {
        [InstructionArgumentDetails(attributeType: AttributeType.RawValue)]
        [InstructionArgumentClientDetails(name: "URL of Details", description: "The URL which contains the detailed version of the Item.")]
        [LoopItem(IDReplacable: "@@ID@@")] // If you change the value of IDReplacable make sure you pass the changes to the file containing it as well!
        public required string LoopItemUrl { get; set; } // Which must contain part like "@@ID@@" see above IDReplacable

        [InstructionArgumentDetails(attributeType: AttributeType.StoreKey)]
        [InstructionArgumentClientDetails(name: "URL", description: "The Store Key which references the previously extracted Table to be processed.")]
        public required string UseResultOf { get; set; }

        [InstructionArgumentDetails(attributeType: AttributeType.RawValue)]
        [InstructionArgumentClientDetails(name: "Identifier Column", description: "The Column Index which refers to the column where the Identifier value of the item is stored.")]
        public required int IdentifierColumnIndex { get; set; }

        [InstructionArgumentDetails(attributeType: AttributeType.CompositeList)]
        [InstructionArgumentClientDetails(name: "Details", description: "Details to fetch.")]
        public List<FieldXPath> Details { get; set; } = new List<FieldXPath>();
    }
}
