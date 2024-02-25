using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapePilot.Helpers
{
    public class ConsolePrinter
    {
        public ConsolePrinter() {
            this.ShouldPrint = false;
        }
        public ConsolePrinter(bool? ShouldPrint = null)
        {
            this.ShouldPrint = ShouldPrint ?? false;
        }

        public bool ShouldPrint { get; set; }

        public void Print(string Message, ConsoleColor color = ConsoleColor.DarkGreen)
        {
            if (ShouldPrint)
            {
                ConsoleColor normal = Console.ForegroundColor;
                Console.ForegroundColor = color;
                Console.WriteLine(Message);
                //Thread.Sleep(2000);
                Console.ForegroundColor = normal;
            }
        }
    }
}
