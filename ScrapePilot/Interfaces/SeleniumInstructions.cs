using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScrapePilot.Models.Instruction.Selenium;

namespace ScrapePilot.Interfaces
{
    public interface SeleniumInstructions
    {
        public void Nav_To(ChromeDriver driver, NavTo arguments);
        public void Perform_Click(ChromeDriver driver, PerformClick arguments);
        public void Switch_Tab(ChromeDriver driver, SwitchTab arguments);
        public string ExtractAttr(ChromeDriver driver, ExtractAttr arguments);
        public void Wait_File_Download(ChromeDriver driver, WaitForDownload args);
        public List<List<string>> GetMultiPageHtmlTableData(ChromeDriver driver, ExtractMultiPageTableData args, InstructionMethodsHtml _htmlActions);
    }
}
