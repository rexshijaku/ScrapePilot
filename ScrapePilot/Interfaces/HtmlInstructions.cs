using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ScrapePilot.Attributes;
using ScrapePilot.Constants.InstructionType;
using ScrapePilot.Models.Instruction.AppDriver;
using ScrapePilot.Models.Instruction.Html;
using ScrapePilot.Models.Recipe;

namespace ScrapePilot.Interfaces
{
    public interface HtmlInstructions
    {
        public HtmlDocument Load(string Url);
        public string GetAttr(HtmlDocument doc, ExtractAttr args);
        public List<List<string>> GetHtmlTableData(HtmlDocument doc, ExtractTableData args);
        public List<List<string>> GetMultiPageHtmlTableData(ExtractMultiPageTableData args, string? dumpListFile = null);
        public string GetTableWithDetails(HtmlDocument doc, LoopTable args);
        public List<KeyValuePair<string, string>> ExtractFields(HtmlDocument doc, List<FieldXPath> fieldXPathList, string trimCharsFromFieldName = " ");
    }
}
