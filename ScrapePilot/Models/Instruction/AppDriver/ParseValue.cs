using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScrapePilot.Attributes;
using ScrapePilot.Constants;
using ScrapePilot.Constants.InstructionType;
using ScrapePilot.Models;

namespace ScrapePilot.Models.Instruction.AppDriver
{
    // TODO Something Like This For Parsing Values Between the Instructions

    //[InstructionDetails(type: HtmlInstuctionType.PARSE_VAL, driver: RecipeDriverType.AppDriver)]
    //public class ParseValue
    //{
    //    [InstructionPropertyDetails(attributeType: AttributeType.StoreKeyOrRawValue)]
    //    [InstructionPropertyClientDetails(name: "Raw", description: "Todo")]
    //    public required string Raw { get; set; }

    //    [InstructionPropertyDetails(attributeType: AttributeType.RawValue)]
    //    [InstructionPropertyClientDetails(name: "Type", description: "Todo")]
    //    public required string Type { get; set; }

    //    public void Parse()
    //    {
    //        if (Store.IsAvailable(Raw))
    //        {
    //            if (Type == DependentFunctions.FileNameFromFullName)
    //            {
    //                Store.SetValue(Raw, Functions.FileName(Raw));
    //            }
    //        }
    //    }
    //}
}
