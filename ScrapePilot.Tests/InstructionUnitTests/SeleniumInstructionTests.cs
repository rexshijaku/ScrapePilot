using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Text;
using ScrapePilot.Constants.InstructionValue.Selenium;
using ScrapePilot.Models.Instruction.Selenium;
using ScrapePilot;

namespace ScrapePilotTests.InstructionTests
{
    public class SeleniumInstructionTests
    {
        ChromeDriver driver;
        ChromeOptions chromeOptions = new ChromeOptions();
        InstructionMethodsSelenium _seleniumActions;

        public SeleniumInstructionTests()
        {
            if (skipBecauseDriveIsNOTPresent())
            {
                return;
            }

            chromeOptions.AddArgument("--headless");
            driver = new ChromeDriver(chromeOptions);
            _seleniumActions = new InstructionMethodsSelenium();
        }

        [Fact]
        public void Launching_A_Browser_With_Url()
        {
            if (skipBecauseDriveIsNOTPresent())
            {
                return;
            }

            // Arrange
            string theUrl = "https://example.com/";

            NavTo navTo = new NavTo()
            {
                Url = theUrl
            };

            string expectedHtmlChunkInPage = "This domain is for use in illustrative examples in documents.";

            // Act
            _seleniumActions.Nav_To(driver, navTo);

            string currentHtmlContent = driver.PageSource;

            driver.Close();

            // Assert
            currentHtmlContent.Should().Contain(expectedHtmlChunkInPage);
        }

        [Fact]
        public void Extract_Attribute_Value()
        {
            if (skipBecauseDriveIsNOTPresent())
            {
                return;
            }

            // Arrange
            NavTo navTo = new NavTo()
            {
                Url = "https://example.com/"
            };

            ExtractAttr extractAttr = new ExtractAttr()
            {
                From = "//a",
                Attr = "href"
            };

            string theExpectedAttrValue = "https://www.iana.org/domains/example";

            // Act
            _seleniumActions.Nav_To(driver, navTo);

            string theExtractedAttrValue = _seleniumActions.ExtractAttr(driver, extractAttr);

            driver.Close();

            // Assert
            theExtractedAttrValue.Should().NotBeNullOrEmpty();
            theExtractedAttrValue.Should().Match(theExpectedAttrValue);
        }

        [Fact]
        public void Perform_Click_On_Element()
        {
            if (skipBecauseDriveIsNOTPresent())
            {
                return;
            }

            // Arrange
            NavTo navTo = new NavTo()
            {
                Url = "https://example.com/"
            };

            PerformClick clickOnElem = new PerformClick()
            {
                On = "//a"
            };

            // The Element is Expected to Navigate us to a new page
            // and we will know if the click is done correctly if the url was changed
            string expectedOutputUrl = "https://www.iana.org/help/example-domains";

            // Act
            _seleniumActions.Nav_To(driver, navTo);

            _seleniumActions.Perform_Click(driver, clickOnElem);

            string currentUrl = driver.Url;
            driver.Close();

            // Assert
            currentUrl.Should().NotBeNullOrEmpty();
            currentUrl.Should().Match(expectedOutputUrl);
        }


        [Fact]
        public void Switching_A_Tab()
        {
            if (skipBecauseDriveIsNOTPresent())
            {
                return;
            }

            // Arrange
            NavTo navTo = new NavTo()
            {
                Url = "https://www.iana.org/help/example-domains"
            };

            SwitchTab switchToTab = new SwitchTab()
            {
                To = SwitchTabVal.LAST
            };

            string expectedNewTabsUrl = "https://example.com/";

            string openNewTabJSScript = $"window.open('{expectedNewTabsUrl}', '_blank');";

            // Act
            _seleniumActions.Nav_To(driver, navTo);

            ((IJavaScriptExecutor)driver).ExecuteScript(openNewTabJSScript);

            _seleniumActions.Switch_Tab(driver, switchToTab);

            string currentUrl = driver.Url;
            driver.Close();

            // Assert
            currentUrl.Should().Match(expectedNewTabsUrl);
        }

        /*
         * Tests wether the file is present in output dir or not
         * We fake a file creation
         */
        [Fact]
        public async void Waiting_For_A_File_Download()
        {
            if (skipBecauseDriveIsNOTPresent())
            {
                return;
            }

            // Arrange
            string outputDir = Path.GetTempPath();

            string filePath = @"xtest-waiting-driver-file.txt";
            string fullPath = Path.Combine(outputDir, filePath);
            string fileContent = "This is the content of the file.";

            WaitForDownload downloadFile = new WaitForDownload()
            { Src = filePath };

            // Act
            Store.Reset();
            Store.SetValue("#" + nameof(AppConfiguration.OutputPath), outputDir);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            WriteFileAsync(fullPath, fileContent); // Intentionally do not await 

            _seleniumActions.Wait_File_Download(driver, downloadFile); // Waits for file to be present in dir

            driver.Close();

            // Assert
            Assert.True(File.Exists(fullPath));
            Assert.True(new FileInfo(fullPath).Length > 0);
            File.ReadAllText(fullPath).Should().Match(fileContent);
        }


        private async Task WriteFileAsync(string filePath, string content)
        {

            Console.WriteLine("Waiting to write :)");
            await Task.Delay(10000);

            // Convert the content to bytes
            byte[] contentBytes = Encoding.UTF8.GetBytes(content);

            // Open or create the file asynchronously
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true))
            {
                // Write the content to the file asynchronously
                await fileStream.WriteAsync(contentBytes, 0, contentBytes.Length);
            }
            Console.WriteLine("Wrote!");

        }

        private bool skipBecauseDriveIsNOTPresent()
        {
            bool b =  !File.Exists(
                Path.Combine(Directory.GetCurrentDirectory(),
                "chromedriver.exe"));

            if (b)
            {
                Console.WriteLine("ChromeDriver.exe was not present!");
            }
            return b;
        }
    }
}
