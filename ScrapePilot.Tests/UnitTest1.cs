namespace ScrapePilotTests
{
    // todo iatcss recepi should be changed
    public class UnitTest1
    {
        // No Need For This!
        [Fact]
        public void Test1()
        {

        }

        //static string theUrl = "https://example.com/";
        //static InstructionMethodsHtml instructionMethodsHtml = new InstructionMethodsHtml();
        //static HtmlDocument doc;

        //static HtmlInstructionTests()
        //{
        //    //doc = instructionMethodsHtml.Load(theUrl);
        //doc.Text.Should().Contain("Smth", Exactly.Once());
        //}
        // dont make too many assertions = don't make it too rigid! (not very strict ?)
        // (not the one build in XUinit but we here, use Fluent Assertions Framework
        // Use this domain for : https://fluentassertions.com/strings/

        //[Fact]

        //public void FetchAndProcessHtml_ReturnsExpectedHtml()
        //{
        //    // Arrange
        //    var html = new Mock<HtmlWeb>();

        //    var mockHtmlWeb = new Mock<InstructionMethodsHtml>(html);

        //    var htmlDocument = new HtmlDocument();
        //    htmlDocument.LoadHtml("<html><body>Hello, World!</body></html>");
        //    mockHtmlWeb.Setup(web => web.Load(It.IsAny<string>())).Returns(htmlDocument);

        //    var htmlFetcher = new InstructionMethodsHtml(mockHtmlWeb.Object.HtmlWeb);

        //    // Act
        //    string result = htmlFetcher.Load("https://example.com").Text;

        //    // Assert
        //    Assert.Equal("<html><body>Hello, World!</body></html>", result);
        //    mockHtmlWeb.Verify(web => web.Load("https://example.com").Text, Times.Once);
        //}
    }
}