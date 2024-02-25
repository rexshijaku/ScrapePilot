using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScrapePilot.Models.Instruction;

namespace ScrapePilot.Constants.InstructionType
{
    /*
     * Provides possible Instruction Types that can be used inside a Recipe
     */
    public class AppDriverInstructionType
    {
        // todo give examples as json for each one
        public const string MOVE_FILE = "move_file";
        public const string DOWNLOAD_A_FILE = "download_a_file";
        public const string SAVE_TXT_FILE = "save_txt_file";
    }
}
