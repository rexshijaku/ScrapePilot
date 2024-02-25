using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScrapePilot.Models.Instruction.AppDriver;

namespace ScrapePilot.Interfaces
{
    public interface AppInstructions
    {
        public string MoveFile(MoveFile args);
        public Task<string> DownloadAFile(DownloadAFile args);
        public string CreateTxtFile(SaveTextFile args);
    }
}
