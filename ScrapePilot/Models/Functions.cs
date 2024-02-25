using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScrapePilot.Attributes;
using ScrapePilot.Constants;

namespace ScrapePilot.Models
{
    public class Functions
    {
        [FunctionDetails(name: DependentFunctions.FileNameFromFullName)]
        public static string FileName(string fullName)
        {
            return Path.GetFileName(fullName);
        }

        [FunctionDetails(name: IndependentFunctions.DateTimeYYYYmmDD)]
        public static string DateTimeyMd()
        {
            return $"{DateTime.Now:yyyyMMdd}";
        }

        [FunctionDetails(name: IndependentFunctions.Space)]
        public static string Blank()
        {
            return " ";
        }
    }
}
