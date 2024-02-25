using ScrapePilot.Attributes;

namespace ScrapePilot.Models.Instruction.Html
{
    // Todo in. details?
    public class FieldXPath
    {

        [InstructionArgumentDetails(attributeType: AttributeType.RawValue)]
        [InstructionArgumentClientDetails(name: "Root", description: "TODO!")]
        public required string Root { get; set; }

        [InstructionArgumentDetails(attributeType: AttributeType.RawValue)]
        [InstructionArgumentClientDetails(name: "FieldName", description: "TODO!")]
        public string? FieldName { get; set; }

        [InstructionArgumentDetails(attributeType: AttributeType.RawValue)]
        [InstructionArgumentClientDetails(name: "Value", description: "TODO!")]
        public required string Value { get; set; }

        [InstructionArgumentDetails(attributeType: AttributeType.RawValue)]
        [InstructionArgumentClientDetails(name: "FixedFieldName", description: "TODO!")]
        public string? FixedFieldName { get; set; }

        [InstructionArgumentDetails(attributeType: AttributeType.RawValue)]
        [InstructionArgumentClientDetails(name: "SplitValueSeparator", description: "TODO!")]
        public string? SplitValueSeparator { get; set; }

        [InstructionArgumentDetails(attributeType: AttributeType.RawValue)]
        [InstructionArgumentClientDetails(name: "JoinValueSeparator", description: "TODO!")]
        public string JoinValueSeparator { get; set; } = ", ";

        [InstructionArgumentDetails(attributeType: AttributeType.RawValue)]
        [InstructionArgumentClientDetails(name: "FallbackIfValueIsMissing", description: "TODO!")]
        public string? FallbackIfValueIsMissing { get; set; }

        [InstructionArgumentDetails(attributeType: AttributeType.RawValue)]
        [InstructionArgumentClientDetails(name: "UseFallbackIfFieldMissing", description: "TODO!")]
        public bool UseFallbackIfFieldMissing { get; set; } = false;

        [InstructionArgumentDetails(attributeType: AttributeType.RawValue)]
        [InstructionArgumentClientDetails(name: "Comment", description: "TODO!")]
        public string? Comment { get; set; }

        /*
         * An item Example used in IATCC Source:
         * {
                "Root": "//div[@class='uk-grid-small']//tr[4]//b",
                "FixedFieldName": "ManagerAddress",
                "Value": "./../following-sibling::td/text()",
                "JoinValueSeparator": ",",
                "FallbackIfValueIsMissing": ""
           }
         */
    }
}
