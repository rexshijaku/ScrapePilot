using ScrapePilot.Attributes;
using ScrapePilot.Constants;
using ScrapePilot.Constants.InstructionType;

namespace ScrapePilot.Models.Instruction.AppDriver
{
    [InstructionDetails(type: AppDriverInstructionType.SAVE_TXT_FILE, driver: RecipeDriverType.AppDriver)]
    public class SaveTextFile
    {
        [InstructionArgumentDetails(attributeType: AttributeType.ListAsRawOrFromStore)]
        [InstructionArgumentClientDetails(name: "To", description: "The Path where the File Should be Saved.")]
        [CanUseInDependentFunction]
        public required List<string> To { get; set; }

        [InstructionArgumentDetails(attributeType: AttributeType.StoreKey)]
        [InstructionArgumentClientDetails(name: "File Source", description: "The Path where the File is Located. Is read from Store. The Value Should be a Store key.")]
        public required string UseContentOf { get; set; }

        [InstructionArgumentDetails(attributeType: AttributeType.RawValue)]
        [InstructionArgumentClientDetails(name: "Delete First", description: "Delete the file-if-exists before saving the new version of it.")]
        public bool DeleteIfExists { get; set; } = true;
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
