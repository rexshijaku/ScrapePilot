using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScrapePilot.Attributes;
using ScrapePilot.Constants;
using ScrapePilot.Constants.InstructionType;

namespace ScrapePilot.Models.Instruction.AppDriver
{
    [InstructionDetails(type: AppDriverInstructionType.MOVE_FILE, driver: RecipeDriverType.AppDriver)]
    public class MoveFile
    {
        [InstructionArgumentDetails(attributeType: AttributeType.StoreKeyOrRawValue)]
        [InstructionArgumentClientDetails(name: "From", description: "The path where file is located.")]
        public required string From { get; set; }

        [InstructionArgumentDetails(attributeType: AttributeType.ListAsRawOrFromStore)]
        [InstructionArgumentClientDetails(name: "To", description: "The new path where file should be moved.")]
        [CanUseDependentFunction(referToField: nameof(From))]
        [CanUseInDependentFunction]
        public List<string>? To { get; set; }

        public Dictionary<string, string> GetArgumentables()
        {
            Dictionary<string, string> mappedArgumentables = new Dictionary<string, string>();
            mappedArgumentables.Add(DependentFunctions.FileNameFromFullName, From);
            return mappedArgumentables;
        }
    }
}
