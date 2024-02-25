using ScrapePilot;
using ScrapePilot.Models.Instruction.AppDriver;

namespace ScrapePilotTests.InstructionIntegrationTests
{
    public class AppDriverIntegrationTests
    {
        /*
         * See if the download + move work
        */
        [Fact]
        public async void Downloading_And_Move_A_File()
        {
            // Arrange
            InstructionMethodsAppDriver instructionMethodsAppDriver = new InstructionMethodsAppDriver();
            string dummyPDFUrl = "https://www.w3.org/WAI/ER/tests/xhtml/testfiles/resources/pdf/dummy.pdf";

            string outputFolder = Path.GetTempPath();
            string downloadPath = Path.Combine(outputFolder, "dummy.pdf");
            string moveToPath = Path.Combine(outputFolder, "integration-tests-dummy-moved.pdf");

            // Act
            await instructionMethodsAppDriver.DownloadAFile(new DownloadAFile()
            {
                From = dummyPDFUrl,
                To = new List<string>() { downloadPath },
                DeleteIfExists = true
            });

            if (File.Exists(moveToPath))
            {
                File.Delete(moveToPath);
            }
            instructionMethodsAppDriver.MoveFile(new MoveFile()
            {
                From = downloadPath,
                To = new List<string>() { moveToPath }
            });

            // Assert
            Assert.True(File.Exists(moveToPath));
            Assert.True(new FileInfo(moveToPath).Length > 0);
        }
    }
}
