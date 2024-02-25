using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapePilot.Models.Recipe
{
    public class Recipe
    {
        public required Use use { get; set; }
        public required List<Instruction> instructions { get; set; }
    }
}
