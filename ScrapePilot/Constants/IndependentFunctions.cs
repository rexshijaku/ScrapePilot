using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapePilot.Constants
{
    /*
     * Provides different function names which have a declaration in some other class (todo).
     */
    public class IndependentFunctions
    {
        [Description("The Current Date in this => 'yyyyMMdd' format.")]
        public const string DateTimeYYYYmmDD = "fn_dateTime-yyyyMMdd";

        [Description("Create a space.")]
        public const string Space = "fn_space";
    }
}
