using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ScrapePilot.Models.Recipe
{
    public class Instruction
    {
        public required string type { get; set; }
        public required dynamic arguments { get; set; }
        public Store? store { get; set; }

        public T GetArguments<T>()
        {
            return JsonSerializer.Deserialize<T>(arguments.ToString());
        }

        public bool resultIsStorable()
        {
            return store != null;
        }

        public void SayPerforming()
        {
            App._consolePrinter.Print($"Performing Instruction: {this.type}...", ConsoleColor.DarkYellow);
        }
    }
}
