using FluentAssertions;
using HtmlAgilityPack;
using System.Text.Json;
using ScrapePilot.Attributes;
using ScrapePilot.Constants;
using ScrapePilot.Helpers;
using ScrapePilot.Models.Instruction.AppDriver;
using ScrapePilot.Models.Instruction.Html;
using ScrapePilot;

namespace ScrapePilotTests.InstructionIntegrationTests
{
    public class HtmlIntegrationTests
    {
        [Fact]
        public async void Download_A_Remote_File_By_Extracted_Attribute_Value()
        {
            // Arrange
            InstructionMethodsHtml instructionMethodsHtml = new InstructionMethodsHtml();
            string outputFilePrefix = "dummy";

            string htmlContent = @"
                    <!DOCTYPE html>
                    <html>
                    <head>
                        <title>My HTML Page</title>
                    </head>
                    <body>
                        <a href='https://www.w3.org/WAI/ER/tests/xhtml/testfiles/resources/pdf/dummy.pdf'>This is a dummy pdf link!</a>
                    </body>
                    </html>
                ";

            ExtractAttr extractAttr = new ExtractAttr()
            {
                Attr = "href",
                From = "//a[text() = \"This is a dummy pdf link!\"]",
                Contraints = new List<string>() { ConstraintType.MUST_BE_PDF }
            };

            // Act

            string storeKeyAttrValue = "#extracted_file_url";

            // load and get attr value
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);

            string attrResult = instructionMethodsHtml.GetAttr(doc, extractAttr);

            string outputFolder = Path.GetTempPath();

            Store.Reset();
            Store.SetValue(storeKeyAttrValue, attrResult);

            // download
            string outputVarNameInRecipe = AttrHelper.GetCustomAttribute<AppVariable>(typeof(AppConfiguration),
                nameof(AppConfiguration.OutputPath)).VariableNameInRecipe;

            Store.SetValue(outputVarNameInRecipe, outputFolder);

            InstructionMethodsAppDriver instructionMethodsAppDriver = new InstructionMethodsAppDriver();
            string outputFile = await instructionMethodsAppDriver.DownloadAFile(new DownloadAFile()
            {
                From = storeKeyAttrValue,
                To = new List<string>() { outputVarNameInRecipe, "fn_fileName-fromFullName" },
                DeleteIfExists = true
            });

            // Assert
            outputFile.Should().NotBeNullOrEmpty();
            new FileInfo(outputFile).Name.Should().StartWith(outputFilePrefix);
            Assert.True(File.Exists(outputFile));
            Assert.True(new FileInfo(outputFile).Length > 0);
        }

        [Fact]
        public void Table_With_Details_Populated_Correctly()
        {
           // TODO
        }

        [Fact]
        public void Extract_Attribute_Value()
        {
            // Arrange
            InstructionMethodsHtml instructionMethodsHtml = new InstructionMethodsHtml();
            string theUrl = "https://example.com/";
            string element = "//a";
            string attr = "href";
            string expectedOutput = "https://www.iana.org/domains/example";

            // Act
            HtmlDocument doc = instructionMethodsHtml.Load(theUrl);

            string attrResult = instructionMethodsHtml.GetAttr(doc,
                new ExtractAttr() { Attr = attr, From = element });

            // Assert
            attrResult.Should().NotBeNullOrEmpty();
            attrResult.Should().Match(expectedOutput);
        }
    }
}
