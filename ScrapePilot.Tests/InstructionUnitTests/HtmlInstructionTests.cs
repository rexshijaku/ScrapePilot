using FluentAssertions;
using HtmlAgilityPack;
using ScrapePilot;
using ScrapePilot.Models.Instruction.Html;

namespace ScrapePilotTests.InstructionUnitTests
{
    public class HtmlInstructionTests
    {
        /*
         * See if the page content will be fetched successfully.
        */
        [Fact]
        public void Fetch_Page_Content()
        {
            string expectedHtmlChunk = "<h1>Example Domain</h1>";

            // Arrange
            InstructionMethodsHtml instructionMethodsHtml = new InstructionMethodsHtml();
            string theUrlOfPageToLoad = "https://example.com/";

            // Act
            HtmlDocument doc = instructionMethodsHtml.Load(theUrlOfPageToLoad);

            // Assert
            doc.Text.Should().NotBeNullOrEmpty();
            doc.Text.Should().Contain(expectedHtmlChunk);
        }

        /*
         * See if the attribute of element is extracted as it should?
         */
        [Fact]
        public void Extract_Attribute_Value()
        {
            // Arrange
            string targetElement = "//a";
            string targetAttribute = "href";
            string expectedOutput = "https://www.domain.name/domains/example";

            string htmlContent = @"
                    <!DOCTYPE html>
                    <html>
                    <head>
                        <title>My HTML Page</title>
                    </head>
                    <body>
                        <a href='https://www.domain.name/domains/example'>This is a text!</a>
                    </body>
                    </html>
                ";

            InstructionMethodsHtml instructionMethodsHtml = new InstructionMethodsHtml();

            // Act
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);

            string attrResult = instructionMethodsHtml.GetAttr(doc,
                new ExtractAttr()
                {
                    From = targetElement,
                    Attr = targetAttribute
                });

            // Assert
            attrResult.Should().NotBeNullOrEmpty();
            attrResult.Should().Match(expectedOutput);
        }

        /*
         * See if the table data is extracted as it should?
         */
        [Fact]
        public void Fetch_Table_Data()
        {
            // todo linkcell path plus other params?

            int expectedRows = 5;
            int skipRowCount = 2;
            int expectedRowsAfterSkip = expectedRows - skipRowCount;
            string rowXPath = "//table[1]//tr";

            // Arrange
            string htmlContent = @" <!DOCTYPE html>
                                        <html>
                                        <head>
                                            <title>People Data</title>
                                        </head>
                                        <body>
                                            <table border='1'>
                                                <th>
                                                    <td>Name</td>
                                                    <td>Occupation</td>
                                                    <td>City</td>
                                                </th>
                                                <tr>
                                                    <td>Person1</td>
                                                    <td>Occupation3</td>
                                                    <td>City5</td>
                                                </tr>
                                                <tr>
                                                    <td>Person2</td>
                                                    <td>Occupation1</td>
                                                    <td>City2</td>
                                                </tr>
                                                <tr>
                                                    <td>Person3</td>
                                                    <td>Occupation4</td>
                                                    <td>City4</td>
                                                </tr>
                                                <tr>
                                                    <td>Person4</td>
                                                    <td>Occupation2</td>
                                                    <td>City3</td>
                                                </tr>
                                                <tr>
                                                    <td>Person5</td>
                                                    <td>Occupation5</td>
                                                    <td>City1</td>
                                                </tr>
                                            </table>
                                        </body>
                                        </html>
                                    ";

            InstructionMethodsHtml instructionMethodsHtml = new InstructionMethodsHtml();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);

            // Act

            // With No Skip
            List<List<string>> resultsNoSkip = instructionMethodsHtml.GetHtmlTableData(doc,
                    new ExtractTableData()
                    {
                        SkipRows = 0,
                        RowXPath = rowXPath,
                        SortColumn = 1
                    });

            // With Skip
            List<List<string>> resultSkip = instructionMethodsHtml.GetHtmlTableData(doc,
                    new ExtractTableData()
                    {
                        SkipRows = skipRowCount,
                        RowXPath = rowXPath,
                        SortColumn = 0
                    });

            // Assert

            // With No Skip
            resultsNoSkip.Count.Should().Be(expectedRows);
            resultsNoSkip[1][0].Should().Match("Person2");
            resultsNoSkip[1][1].Should().Match("Occupation1");
            resultsNoSkip[1][2].Should().Match("City2");

            // With Skip
            resultSkip.Count.Should().Be(expectedRowsAfterSkip);
            resultSkip[0][0].Should().Match("Person3");
            resultSkip[0][1].Should().Match("Occupation4");
            resultSkip[0][2].Should().Match("City4");
        }

        /*
        * See if the multip page table data is extracted as it should?
        */
        [Fact]
        public void Fetch_MultiPage_Table_Data()
        {
            // TODO
        }

        /*
        * See if the fields specified with xpaths are extracted as it should?
        */
        [Fact]
        public void Extract_Fields()
        {
            int expectedItems = 4; // h1 + 3xdivs

            string htmlContent = @" <!DOCTYPE html>
                                        <html>
                                        <head>
                                          
                                        </head>
                                        <body>
                                            <h3>ExampleName</h3>
                                            <div class='uk-grid-small'>
                                                <div>
                                                      <b>ID</b> 10009<br>
                                                      <b>Name</b> Nejm<br>
                                                      <b>Surname</b> Surnejm<br>
                                                </div>
                                            </div>
                                        </body>
                                        </html>
                                    ";

            // Arrange
            InstructionMethodsHtml instructionMethodsHtml = new InstructionMethodsHtml();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);

            List<FieldXPath> fields = new List<FieldXPath>() {

                        new FieldXPath() {
                            Root = "//h3",
                            Value= "./text()",
                            FixedFieldName ="Name"
                        },
                        new FieldXPath()
                        {
                            Root = "//div[@class=\"uk-grid-small\"]//div/b",
                            Value = "./following-sibling::text()[1]",
                            FieldName = "./text()"
                        }
                    };

            // Act
            List<KeyValuePair<string, string>> result = instructionMethodsHtml.ExtractFields(doc, fields);

            // Assert
            result.Count.Should().Be(expectedItems);
            result[0].Key.Should().Match("Name");
            result[0].Value.Should().Match("ExampleName");

            result[1].Key.Should().Match("ID");
            result[1].Value.Should().Match("10009");

            result[3].Key.Should().Match("Surname");
            result[3].Value.Should().Match("Surnejm");

        }
    }
}
