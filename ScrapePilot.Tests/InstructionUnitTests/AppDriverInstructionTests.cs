using FluentAssertions;
using ScrapePilot;
using ScrapePilot.Models.Instruction.AppDriver;

namespace ScrapePilotTests.InstructionUnitTests
{
    public class AppDriverInstructionTests
    {
        /*
         * See if the remote file will be downlaoded successfully.
        */
        [Fact]
        public async void Downloading_A_File_OK()
        {
            // Arrange
            InstructionMethodsAppDriver instructionMethodsAppDriver = new InstructionMethodsAppDriver();
            string theUrl = "https://www.w3.org/WAI/ER/tests/xhtml/testfiles/resources/pdf/dummy.pdf";
            string downloadPath = Path.Combine(Path.GetTempPath(), "dummy.pdf");

            // Act
            await instructionMethodsAppDriver.DownloadAFile(new DownloadAFile()
            {
                From = theUrl,
                To = new List<string>() { downloadPath },
                DeleteIfExists = true
            });

            // Assert
            Assert.True(File.Exists(downloadPath));
            Assert.True(new FileInfo(downloadPath).Length > 0);
        }

        /*
        * See if the file will be moved successfully.
       */
        [Fact]
        public void Moving_A_File()
        {
            // Arrange
            string fileContent = "This is a dummy file content.";
            string tempFolderPath = Path.GetTempPath();
            string dummyFilePathFrom = Path.Combine(tempFolderPath, "xunit-dummy.txt");
            // Create a temporary file for test
            File.WriteAllText(dummyFilePathFrom, fileContent);
            string dummyFilePathTo = Path.Combine(tempFolderPath, "xunit-dummy_moved.txt");

            InstructionMethodsAppDriver instructionMethodsAppDriver = new InstructionMethodsAppDriver();

            // Act

            // To test the move remove if exists first
            if (File.Exists(dummyFilePathTo))
            {
                File.Delete(dummyFilePathTo);
            }
            instructionMethodsAppDriver.MoveFile(new MoveFile()
            {
                From = dummyFilePathFrom,
                To = new List<string>() { dummyFilePathTo }
            });

            // Assert
            Assert.True(File.Exists(dummyFilePathTo));
            Assert.True(new FileInfo(dummyFilePathTo).Length > 0);
            File.ReadAllText(dummyFilePathTo).Should().Match(fileContent);
        }

        /*
         * See if the file will be created successfully.
        */
        [Fact]
        public void Creating_A_File()
        {
            // Arrange
            InstructionMethodsAppDriver instructionMethodsAppDriver = new InstructionMethodsAppDriver();
            string storeKey = "#xunit-key-1";
            string fileContent = "MyValue";
            string createdPath = Path.Combine(Path.GetTempPath(), "xunit-test.txt");

            Store.Reset();
            Store.SetValue(storeKey, fileContent);

            SaveTextFile args = new SaveTextFile()
            {
                To = new List<string>() { createdPath },
                UseContentOf = storeKey,
                DeleteIfExists = true
            };

            // Act
            instructionMethodsAppDriver.CreateTxtFile(args);

            // Assert
            Assert.True(File.Exists(createdPath));
            Assert.True(new FileInfo(createdPath).Length > 0);
            File.ReadAllText(createdPath).Should().Match(fileContent);
        }
    }
}
