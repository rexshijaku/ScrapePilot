using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using ScrapePilot.Constants.InstructionValue.Selenium;
using ScrapePilot.Interfaces;
using ScrapePilot.Models.Instruction.Selenium;
using ScrapePilot.Constants;
using HtmlAgilityPack;
using System.Text;
using ScrapePilot.Attributes;
using ScrapePilot.Helpers;

namespace ScrapePilot
{
    /*
     * Selenium Driver - Includes all methods that are specific to Selenium
     */
    public class InstructionMethodsSelenium : SeleniumInstructions
    {
        public const int WAIT_TIME = 3000;

        /*
         * TODO Move this function to an interface?! or an inheretable class
         */
        public ChromeOptions GetDriverConfigs(Models.Configs.SeleniumDriverConfigs? configs, string output_path)
        {
            ChromeOptions? options = new ChromeOptions();
            options.AddUserProfilePreference("download.default_directory", output_path);
            options.AddArguments("--window-size=1920,1080"); // default (some pages need this page)
            options.PageLoadStrategy = PageLoadStrategy.Normal;

            bool headLess = true; // by default is enabled

            if (configs != null) // if any config is set to stop such 
            {
                headLess = configs.Headless ?? true;
            }

            if (headLess)
            {
                options.AddArguments("headless");
            }

            return options;
        }

        public void Nav_To(ChromeDriver driver, NavTo arguments)
        {
            try
            {
                App._consolePrinter.Print($"Navigating to Url: {arguments.Url}");
                driver.Navigate().GoToUrl(arguments.Url);
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains(" timed out "))
                {
                    throw;
                }

                App._consolePrinter.Print($"{DateTime.Now} SeleniumWaitForPageAndFile: Timeout after GoToUrl()");
            }

            App._consolePrinter.Print($"Navigating to Url Instruction ended.");
        }

        public void Perform_Click(ChromeDriver driver, PerformClick arguments)
        {
            IWebElement? element = null;
            var waitGoToDownloadPageBtnLoad = new Func<IWebDriver, bool>(d =>
            {
                try
                {
                    App._consolePrinter.Print($"Performing Click On: {arguments.On}");
                    element = driver.FindElement(By.XPath(arguments.On));
                    return true;
                }
                catch
                {
                    App._consolePrinter.Print(string.Format("Failed to find the element {0}. Trying again...Waiting to load...", arguments.On));
                    App._consolePrinter.Print($"Performing Click Instruction ended.");
                    return false;
                }
            });

            WaitForDriver(driver, waitGoToDownloadPageBtnLoad, verbose: true);

            bool performed = false;

            int maxTryToClick = 5;
            int triedToClick = 0;
            while (!performed)
            {
                try
                {
                    App._consolePrinter.Print($"Trying to click the New Found Element: {triedToClick}");
                    triedToClick++;

                    // click as an action, otherwise not clickable
                    new Actions(driver).MoveToElement(element).Click().Perform();
                    performed = true;
                    break;
                }
                catch
                {
                    if (triedToClick == maxTryToClick)
                    {
                        break;
                    }
                    Thread.Sleep(3000);
                }
            }

            App._consolePrinter.Print($"Trying to click the New Found Element Instruction ended.");
        }

        public List<List<string>> GetMultiPageHtmlTableData(ChromeDriver driver, ExtractMultiPageTableData arguments, InstructionMethodsHtml _htmlActions)
        {
            List<List<string>> table = new List<List<string>>();

            IWebElement? btnToClick = driver.FindElementByXPath(arguments.PageChanger);

            string theValueForLoadCondition = driver.FindElement(By.XPath(arguments.ChangeIsDetectedWhenThisElementsValueChanged)).Text;
            bool isInitial = true;

            bool isDone = false;
            int totalSame = 0;

            var waitForPageChange = new Func<IWebDriver, bool>(d =>
            {
                try
                {
                    App._consolePrinter.Print($"Checking If Page is Loaded according to Elem: {arguments.ChangeIsDetectedWhenThisElementsValueChanged}");
                    IWebElement? element = driver.FindElement(By.XPath(arguments.ChangeIsDetectedWhenThisElementsValueChanged));
                    if (element.Text != theValueForLoadCondition || isInitial)
                    {
                        isInitial = false;
                        theValueForLoadCondition = element.Text;
                        Console.WriteLine($"The new Value of Tracked Element is {theValueForLoadCondition}");
                        return true;
                    }
                    else
                    {
                        isDone = true; // same text found twice? then stop it
                        return true;
                    }
                }
                catch
                {
                    App._consolePrinter.Print(string.Format("Failed to find the element {0}. Trying again...Waiting to load...", arguments.ChangeIsDetectedWhenThisElementsValueChanged));
                    App._consolePrinter.Print($"The check in Page Load is done!");
                    totalSame++;

                    return false;
                }
            });

            // walk through pages

            while (!isDone)
            {
                // wait for the page change

                WaitForDriver(driver, waitForPageChange, verbose: true);
                if (isDone)
                {
                    break;
                }

                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(driver.PageSource);
                List<List<string>> tblData = _htmlActions.GetHtmlTableData(htmlDocument, arguments.TableDataXPath);
                table.AddRange(tblData);

                // if clicked button was disabled and set to check then stop the loop
                var btnToClickTmp = driver.FindElementByXPath(arguments.PageChanger);

                if (arguments.StopWhenElemIsDisabled && btnToClickTmp != null && !btnToClickTmp.Enabled)
                {
                    break;
                }
               
                new Actions(driver).MoveToElement(btnToClick).Click().Perform();

                Thread.Sleep(3000);
            }

            App._consolePrinter.Print($"Trying to click the New Found Element Instruction ended.");

            return table;

        }

        public void Switch_Tab(ChromeDriver driver, SwitchTab arguments)
        {
            App._consolePrinter.Print($"Switching Tab To: {arguments.To}");

            if (arguments.To == SwitchTabVal.FIRST)
            {
                driver.SwitchTo().Window(driver.WindowHandles.First());
            }
            else  // todo ?
            {
                driver.SwitchTo().Window(driver.WindowHandles.Last());
            }

            App._consolePrinter.Print($"Switching Tab Instruction ended.");
        }

        public string ExtractAttr(ChromeDriver driver, ExtractAttr arguments)
        {
            App._consolePrinter.Print($"Extracting Attribute {arguments.Attr} From: {arguments.From}");
            string attr_val = string.Empty;
            var waitBtnLoad = new Func<IWebDriver, bool>(d =>
            {
                try
                {
                    // find valid text btn and the filen name
                    var txtBtnTmp = driver.FindElement(By.XPath(arguments.From));
                    attr_val = txtBtnTmp.GetAttribute(arguments.Attr);
                    if (arguments.Contraints != null)
                    {
                        Handle_Constraints(arguments.Contraints, attr_val);
                    }
                    return true;
                }
                catch
                {
                    App._consolePrinter.Print(string.Format("Failed to find the element {0}. Retrying... Waiting to load...", arguments.From));
                    return false;
                }
            });


            WaitForDriver(driver, waitBtnLoad, verbose: true);

            App._consolePrinter.Print($"Extracting Attribute Instruction end. The Result: {attr_val}");

            return attr_val;
        }

        public void Wait_File_Download(ChromeDriver driver, WaitForDownload args)
        {
            if (Store.IsAvailable(args.Src))
            {
                args.Src = Store.GetValue(args.Src);
            }

            string output_path = AttrHelper.GetCustomAttribute<AppVariable>(typeof(AppConfiguration),
                nameof(AppConfiguration.OutputPath)).VariableNameInRecipe;

            string path = Path.Combine(Store.GetValue(output_path), args.Src);

            var waitDownload = new Func<IWebDriver, bool>(d =>
            {
                bool fileExists = File.Exists(path);
                if (!fileExists)
                {
                    App._consolePrinter.Print("Waiting file to be downloaded..." + path);
                }
                return fileExists;
            });

            WaitForDriver(driver, waitDownload, verbose: true);
        }

        private bool WaitForDriver(IWebDriver driver, Func<IWebDriver, bool>? checkWebDriverStatus = null, bool verbose = true)
        {
            int maxWaitSeconds = 300;
            int interval = 10;

            App._consolePrinter.Print($"{DateTime.Now} WaitForDriver: Waiting {maxWaitSeconds}sec");

            var webDriverWait = new WebDriverWait(driver, TimeSpan.FromSeconds(interval));
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            while (stopwatch.Elapsed.TotalSeconds < maxWaitSeconds)
            {
                try
                {
                    webDriverWait.Until(checkWebDriverStatus);
                    break;
                }
                catch (WebDriverException wex)  // Timeout is normal here
                // catch (OpenQA.Selenium.WebDriverTimeoutException wex)  // Timeout is normal here
                {
                    // No action, a timeout is expected here
                    if (wex.Message.ToLower().Contains("timed out"))
                    {
                        continue;
                    }
                    else
                    {
                        throw;
                    }
                }
                catch    // Unexpected error
                {
                    throw;
                }
            }
            stopwatch.Stop();
            if (stopwatch.Elapsed.TotalSeconds < maxWaitSeconds)
            {
                App._consolePrinter.Print($"Ok waiting for driver: {stopwatch.Elapsed}");
                return true;
            }
            else
            {
                App._consolePrinter.Print($"Timed out waiting for driver: {stopwatch.Elapsed}");
                return false;
            }
        }

        private void Handle_Constraints(List<string> constrains, string value)
        {
            App._consolePrinter.Print($"Handling Constrains: " + String.Join(",", constrains));

            bool result = true;
            string message = String.Empty;

            foreach (string con in constrains)
            {
                if (con == ConstraintType.MUST_BE_PDF)
                {
                    if (!value.EndsWith("pdf"))
                    {
                        result = false;
                        message = $"The proper File Type .pdf was not read.";
                        break;
                    }
                }

                if (con == ConstraintType.MUST_BE_CSV)
                {
                    if (!value.EndsWith("csv"))
                    {
                        result = false;
                        message = $"The proper File Type .csv was not read.";
                        break;
                    }
                }

                if (con == ConstraintType.MUST_BE_XLS)
                {
                    if (!value.EndsWith("xls"))
                    {
                        result = false;
                        message = $"The proper File Type .xls was not read.";
                        break;
                    }
                }
            }

            App._consolePrinter.Print($"Handling Constrains Done.");

            if (!result)
            {
                throw new Exception(message);
            }
        }

    }
}
