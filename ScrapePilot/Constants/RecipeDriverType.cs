using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapePilot.Constants
{
    /*
     * Provides Recipe Driver Types which can be used inside a Recipe use command.
     */
    public class RecipeDriverType
    {
        public const string SELENIUM = "Selenium";
        public const string HtmlAguilitiPack = "HTMLap";
        public const string AppDriver = "AppDriver";

        public static List<string> GetAll()
        {
            return new List<string> { SELENIUM, HtmlAguilitiPack, AppDriver };
        }
    }
}
