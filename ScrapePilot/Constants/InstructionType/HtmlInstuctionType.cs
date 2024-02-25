using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapePilot.Constants.InstructionType
{
    public class HtmlInstuctionType
    {
        public const string LOAD_PAGE = "load";
        public const string EXTRACT_ATTR = "extract_attr";
        public const string EXTRACT_TABLE_DATA = "extract_table";
        public const string EXTRACT_TABLE_XPATH_LIST = "extract_table_xpath_list";
        public const string EXTRACT_MULTI_PAGE_TABLE_DATA = "extract_multi_page_table";
        public const string LOOP_A_TABLE = "loop_table";
        public const string PARSE_VAL = "parse_val";
    }
}
