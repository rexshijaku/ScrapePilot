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
    [InstructionDetails(type: AppDriverInstructionType.DOWNLOAD_A_FILE, driver: RecipeDriverType.AppDriver)]
    public class DownloadAFile
    {
        [InstructionArgumentDetails(attributeType: AttributeType.StoreKeyOrRawValue)]
        [InstructionArgumentClientDetails(name: "URL", description: "The Source URL where the File resides and from where is intended to be downloaded.")]
        public required string From { get; set; }

        [InstructionArgumentDetails(attributeType: AttributeType.ListAsRawOrFromStore)]
        [InstructionArgumentClientDetails(name: "Save To", description: "The Path where the Downloaded File should be stored." +
            "A list of strings which when combined create the full path.")]
        [CanUseDependentFunction(referToField: nameof(From))]
        [CanUseInDependentFunction]
        public required List<string> To { get; set; }

        [InstructionArgumentDetails(attributeType: AttributeType.RawValue)]
        [InstructionArgumentClientDetails(name: "Delete First", description: "Before Saving, Delete The File (if exists) with the same name in The <Download Folder>.")]
        public bool DeleteIfExists { get; set; } = true;

        public Dictionary<string, string> GetArgumentables()
        {
            Dictionary<string, string> mappedArgumentables = new Dictionary<string, string>();
            mappedArgumentables.Add(DependentFunctions.FileNameFromFullName, From);
            return mappedArgumentables;
        }
    }
}
