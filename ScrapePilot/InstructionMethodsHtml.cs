using HtmlAgilityPack;
using System.Reflection;
using System.Text;
using System.Text.Json;
using ScrapePilot.Attributes;
using ScrapePilot.Constants.InstructionType;
using ScrapePilot.Interfaces;
using ScrapePilot.Models.Instruction.Html;

namespace ScrapePilot
{
    /*
     * HTML Driver - Includes all methods that are specific to HtmlAguilityPack
     */
    public class InstructionMethodsHtml : HtmlInstructions
    {
        public HtmlWeb HtmlWeb { get; set; }
        public InstructionMethodsHtml()
        {
            HtmlWeb = new HtmlWeb();
        }

        public InstructionMethodsHtml(HtmlWeb htmlWeb)
        {
            this.HtmlWeb = htmlWeb;
        }

        /*
         * TODO Move this function to an interface?! or an inheretable class
         */
        public void ApplyDriverConfigs(Models.Configs.HTMLDriverConfigs? configs)
        {
            if (configs != null)
            {
                if (!string.IsNullOrEmpty(configs.EncodingName))
                {
                    HtmlWeb.OverrideEncoding = Encoding.GetEncoding(configs.EncodingName);
                }
            }
        }

        public HtmlDocument Load(string Url)
        {
            App._consolePrinter.Print($"Loading Url: {Url}");
            HtmlDocument document = HtmlWeb.Load(Url);
            App._consolePrinter.Print($"Document Loaded.");
            return document;
        }

        public string GetAttr(HtmlDocument doc, ExtractAttr args)
        {
            App._consolePrinter.Print($"Getting the Attribute {args.Attr} From the Element: {args.From}");
            string retval = string.Empty;
            var node = doc.DocumentNode.SelectSingleNode(args.From);
            if (node != null)
            {
                retval = node.Attributes[args.Attr].Value;
                if (!retval.ToLower().StartsWith("http:") && !retval.ToLower().StartsWith("https:"))
                {
                    retval = new Uri(HtmlWeb.ResponseUri.Scheme + "://" + HtmlWeb.ResponseUri.Host + "/" + retval).ToString();
                }
                App._consolePrinter.Print($"The Attribute Value: " + retval);
            }
            else
            {
                App._consolePrinter.Print($"The Element Was Not Found: " + args.From);
            }
            App._consolePrinter.Print($"The Attribute Value Retrival is Done");
            return retval;
        }

        public List<List<string>> GetHtmlTableData(HtmlDocument doc, ExtractTableData args)
        {
            //var doc = string.IsNullOrWhiteSpace(url) ? this.HtmlDocument : HtmlWeb.Load(url);

            App._consolePrinter.Print($"Getting the Table Data. From Rows: {args.RowXPath}");
            List<List<string>> list = GetList(HtmlWeb, doc, args.RowXPath, args.SkipRows, args.RowLimit, args.LinkCellPath);
            if (args.SortColumn > 0)
            {
                App._consolePrinter.Print($"The Table Data Is Being Sorted by Column: {args.SortColumn}.");
                list = list.OrderBy(o => o[args.SortColumn - 1]).ToList();
            }
            App._consolePrinter.Print($"The Table Data Was Extracted.");
            return list;
        }

        public List<KeyValuePair<string, string>> ExtractFields(HtmlDocument doc, List<FieldXPath> fieldXPathList, string trimCharsFromFieldName = " ")
        {
            App._consolePrinter.Print($"Extracting #{fieldXPathList.Count} Fields.");
            char[] trimChars = trimCharsFromFieldName.ToCharArray();
            //var doc = string.IsNullOrWhiteSpace(url) ? this.HtmlDocument : HtmlWeb.Load(url);
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            foreach (FieldXPath xpathItem in fieldXPathList)
            {
                var fieldNodes = doc.DocumentNode.SelectNodes(xpathItem.Root);
                if (fieldNodes?.Count > 0)
                {
                    foreach (var fieldNode in fieldNodes)
                    {
                        var fieldName = xpathItem.FixedFieldName ?? CleanString(fieldNode.InnerText).Trim(trimChars);
                        // var fieldName = xpathItem.FixedFieldName ?? fieldNode.InnerText.Trim();
                        var valueNodes = fieldNode.SelectNodes(xpathItem.Value);
                        if (valueNodes?.Count > 0)
                        {
                            var value = string.Join(xpathItem.JoinValueSeparator, valueNodes.Select(s => CleanString(s.InnerText)).Where(w => !string.IsNullOrWhiteSpace(w)));
                            // var value = string.Join(xpathItem.JoinValueSeparator, valueNodes.Select(s => s.InnerText.Trim()));
                            list.Add(new KeyValuePair<string, string>(fieldName, value));
                        }
                        else if (xpathItem.FallbackIfValueIsMissing != null)
                        {
                            list.Add(new KeyValuePair<string, string>(fieldName, xpathItem.FallbackIfValueIsMissing));
                        }
                    }
                }
                else if (xpathItem.UseFallbackIfFieldMissing)
                {
                    list.Add(new KeyValuePair<string, string>(xpathItem.FixedFieldName ?? "", xpathItem.FallbackIfValueIsMissing ?? String.Empty));
                }
            }
            App._consolePrinter.Print($"Fields #{fieldXPathList.Count} were extracted successfully.");
            return list;
        }

        public List<List<string>> GetMultiPageHtmlTableData(ExtractMultiPageTableData args, string? dumpListFile = null)
        {
            App._consolePrinter.Print($"Extracting Multi Page Table Data.");

            List<List<string>> list = new List<List<string>>();
            int lastChecksum = 0;
            double lastListChecksum = 0;
            while (true)
            {
                string tmpUrl = args.LoopableUrl.Replace("@@OFFSET@@", args.PaggingOffset.ToString());
                HtmlDocument doc = Load(tmpUrl);
                int checksum = doc.DocumentNode.OuterHtml.GetHashCode();
                if (checksum == lastChecksum)
                {
                    break;
                }
                List<List<string>> tmpList = GetList(HtmlWeb, doc, args.RowXPath, args.SkipRows, args.RowLimit, args.LinkCellPath);
                if (tmpList.Count == 0)
                {
                    break;
                }
                double listChecksum = tmpList.Average(s => s.Average(s2 => s2.GetHashCode()));
                if (listChecksum == lastListChecksum)
                {
                    break;
                }
                lastListChecksum = listChecksum;
                if (!string.IsNullOrWhiteSpace(dumpListFile))
                {
                    var pages = tmpList.Select(s => string.Join("\t", s));
                    string str = string.Join("\r\n", pages) + "\r\n";
                    File.AppendAllText(dumpListFile, str);
                }

                foreach (List<string> item in tmpList)
                {
                    list.Add(item);

                    if (args.RowLimit > 0 && list.Count == args.RowLimit)
                    {
                        App._consolePrinter.Print($"Multi Page Table Data: The Row Limit of {args.RowLimit} was reached.");
                        break;
                    }
                }

                args.PaggingOffset += args.IncreaseOffset;
                lastChecksum = checksum;
                // todo args.PageMinLimit
                if ((args.PaggingOffset < (args.PageMinLimit)) || (args.PaggingOffset > (args.PageMaxLimit - 1)))
                {
                    break;
                }
            }

            if (args.SortColumn > 0)
            {
                App._consolePrinter.Print($"Sorting the Extracted Multi Page Table Data.");
                list = list.OrderBy(o => o[args.SortColumn - 1]).ToList();
            }

            App._consolePrinter.Print($"Multi Page Table Data Extracted.");

            return list;
        }

        private List<List<string>> GetList(HtmlWeb htmlWeb, HtmlDocument doc, string rowPath, int? skipRows, int? rowLimit, string? linkCellPath = null)
        {
            List<List<string>> list = new List<List<string>>();
            var rowNodes = doc.DocumentNode.SelectNodes(rowPath);
            if (rowNodes == null)
            {
                return list;
            }
            int rowNo = 0;
            foreach (var rowNode in rowNodes)
            {
                rowNo++;
                if (skipRows > 0 && rowNo <= skipRows)
                {
                    continue;
                }
                HtmlNodeCollection? cellNodes = rowNode.SelectNodes("td");
                if (cellNodes == null && rowNo == 1)
                {
                    cellNodes = rowNode.SelectNodes("th");
                }
                // List<string> innerList = cellNodes.Select(s => s.InnerText.Trim().Replace("\r\n", "; ")).ToList();
                if (cellNodes != null)
                {
                    List<string> innerList = cellNodes.Select(s => GetNodeText(s, "; ").Replace("\r\n", "; ")).ToList();
                    if (!string.IsNullOrWhiteSpace(linkCellPath))
                    {
                        var node = rowNode.SelectSingleNode(linkCellPath);
                        string href = "";
                        if (node != null)
                        {
                            href = node.Attributes["href"].Value;
                            if (!href.ToLower().StartsWith("http:") && !href.ToLower().StartsWith("https:"))
                            {
                                href = new Uri(htmlWeb.ResponseUri, href).ToString();
                            }
                        }
                        innerList.Add(href);
                    }
                    list.Add(innerList);
                }

                if (rowLimit > 0 && list.Count == rowLimit)
                {
                    App._consolePrinter.Print($"The Row Limit of {rowLimit} was reached.");
                    break;
                }
            }

            return list;
        }

        public string GetTableWithDetails(HtmlDocument doc, LoopTable args)
        {
            if (!Store.IsAvailable(args.UseResultOf))
            {
                throw new Exception($"Invalid Action Argument Declaration. {args.UseResultOf} in GetTableDetails/{HtmlInstuctionType.EXTRACT_TABLE_DATA} was not set.");
            }

            // get the result from a step before
            List<List<string>>? tableRows = JsonSerializer.Deserialize<List<List<string>>>(Store.GetValue(args.UseResultOf));

            List<string> resultTableHeaders = new List<string>();
            List<List<KeyValuePair<string, string>>> resultTableDetails = new List<List<KeyValuePair<string, string>>>();

            int count = 0;

            foreach (List<string> row in tableRows ?? new List<List<string>>())
            {
                PropertyInfo? propertyInfo = typeof(LoopTable).GetProperty(nameof(args.LoopItemUrl));

                if (propertyInfo != null)
                {
                    LoopItemAttribute? dataAttribute = (LoopItemAttribute?)Attribute.GetCustomAttribute(propertyInfo, typeof(LoopItemAttribute));

                    if (dataAttribute != null)
                    {
                        string url = args.LoopItemUrl.Replace(dataAttribute.IDReplacable, row[args.IdentifierColumnIndex]);

                        count++;

                        doc = Load(url);

                        List<KeyValuePair<string, string>> vesselDetails = ExtractFields(doc, args.Details, trimCharsFromFieldName: " :");
                        resultTableDetails.Add(vesselDetails);
                        resultTableHeaders.AddRange(vesselDetails.Select(s => s.Key).Where(w => !resultTableHeaders.Contains(w)));

                        if (count % 10 == 0)
                        {
                            Console.WriteLine($"{count} / {tableRows?.Count} looped table items...");
                        }
                    }
                }
            }

            string tblHeader = string.Join("\t", resultTableHeaders);
            string tblBody = string.Join(Environment.NewLine, resultTableDetails.Select(r => string.Join("\t",
                resultTableHeaders.Select(h => r.Where(w => w.Key == h).Select(s => s.Value).FirstOrDefault() ?? ""))));
            string table = tblHeader + Environment.NewLine + tblBody;

            return table;
        }

        private static string CleanString(string str, string? newLineReplacement = null)
        {
            string retval = System.Web.HttpUtility.HtmlDecode(str);
            if (newLineReplacement != null)
            {
                retval = retval.Replace("\r\n", newLineReplacement);
                retval = retval.Replace("\n", newLineReplacement);
            }
            //retval = retval.Replace("&nbsp;", " ");
            //retval = retval.Replace("&amp;", "&");
            //retval = retval.Replace("&lt;", "<");
            //retval = retval.Replace("&gt;", ">");
            retval = retval.Trim();
            return retval;
        }

        private string GetNodeText(HtmlNode htmlNode, string? newLineReplacement = null)
        {
            string retval = String.Empty;
            StringBuilder sb = new StringBuilder();
            var nodes = htmlNode.SelectNodes(".//text()");
            if (nodes != null)
            {
                foreach (HtmlNode node in nodes)
                {
                    if (!string.IsNullOrWhiteSpace(node.InnerText))
                    {
                        sb.Append(node.InnerText.Trim() + " ");
                    }
                }
                retval = sb.ToString().Trim();
                retval = CleanString(retval, newLineReplacement);
            }
            return retval;
        }

        // todo check the src part what for?
        private string GetLinkFromPage(string pageUrl, string anchorPath, string? attribute = null)
        {
            string retval = "";
            if (string.IsNullOrWhiteSpace(attribute))
            {
                if (anchorPath.EndsWith("@src"))
                {
                    attribute = "src";
                }
                else
                {
                    attribute = "href";
                }
            }
            var doc = HtmlWeb.Load(pageUrl);
            var node = doc.DocumentNode.SelectSingleNode(anchorPath);
            if (node != null)
            {
                retval = node.Attributes[attribute].Value;
                if (!retval.ToLower().StartsWith("http:") && !retval.ToLower().StartsWith("https:"))
                {
                    retval = new Uri(HtmlWeb.ResponseUri.Scheme + "://" + HtmlWeb.ResponseUri.Host + "/" + retval).ToString();
                }
            }
            return retval;
        }

    }
}
