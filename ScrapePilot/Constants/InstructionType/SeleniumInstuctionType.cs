using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapePilot.Constants.InstructionType
{
    public class SeleniumInstuctionType
    {
        public const string NAVIGATE_TO = "nav_to";
        public const string PERFORM_CLICK = "perform_click";
        public const string EXTRACT_MULTI_PAGE_TABLE_DATA = "extract_multi_page_table_data";

        // todo add atribute and point class to instroction value
        public const string SWITCH_TAB = "switch_tab";

        public const string EXTRACT_ATTR = "extract_attr";
        public const string WAIT_FILE_DOWNLOAD = "wait_file_download";
    }
}
