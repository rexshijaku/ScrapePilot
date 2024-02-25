using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapePilot.Constants
{
    /*
     * Provides different Constraint types that can be applied to certain cases and have the implementation somewhere else (todo). 
     */
    public class ConstraintType
    {
        public const string MUST_BE_PDF = "must_be_pdf_file";
        public const string MUST_BE_CSV = "must_be_csv_file";
        public const string MUST_BE_XLS = "must_be_xls_file";
    }
}
