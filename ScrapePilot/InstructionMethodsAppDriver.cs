using ScrapePilot.Helpers;
using ScrapePilot.Interfaces;
using ScrapePilot.Models.Instruction.AppDriver;

namespace ScrapePilot
{
    /*
     * App Driver - Includes all methods that are not specific to HtmlAguilitPack or Selenium
     */
    public class InstructionMethodsAppDriver : AppInstructions
    {
        public async Task<string> DownloadAFile(DownloadAFile args)
        {
            if (Store.IsAvailable(args.From))
            {
                args.From = Store.GetValue(args.From);
            }

            FileHelper.SetArgumentables(args.To, args.GetArgumentables());
            string saveToPath = FileHelper.GetFragmented(args.To);

            if (args.DeleteIfExists && File.Exists(saveToPath))
            {
                File.Delete(saveToPath);
            }

            App._consolePrinter.Print($"Downloading a file {args.From} and saving To: {saveToPath}");

            var httpClient = new HttpClient();
            httpClient.Timeout = Timeout.InfiniteTimeSpan;
            var httpResult = await httpClient.GetAsync(args.From);
            using var resultStream = await httpResult.Content.ReadAsStreamAsync();
            using var fileStream = File.Create(saveToPath);
            resultStream.CopyTo(fileStream);

            App._consolePrinter.Print($"Downloading The File Instruction end.");
            return saveToPath;
        }
        
        public string MoveFile(MoveFile args)
        {
            // parse local / argument dependent functions first
            if (args.To == null)
            {
                args.To = new List<string>();
            }

            string sourceVal = args.From;

            if (Store.IsAvailable(args.From))
            {
                args.From = Store.GetValue(args.From);
            }

            FileHelper.SetArgumentables(args.To, args.GetArgumentables());

            string move_to = FileHelper.GetFragmented(args.To);

            App._consolePrinter.Print($"Moving a file From {sourceVal} To: {move_to}");
            File.Move(sourceVal, move_to);
            App._consolePrinter.Print($"Moving completed!");
            return move_to;
        }

        public string CreateTxtFile(SaveTextFile args)
        {
            string savePath = FileHelper.GetFragmented(args.To);

            if (args.DeleteIfExists && File.Exists(savePath))
            {
                File.Delete(savePath);
            }

            App._consolePrinter.Print($"Creating a Text file {savePath} using the Content of: {args.UseContentOf}");
            File.WriteAllText(savePath, Store.GetValue(args.UseContentOf));
            App._consolePrinter.Print($"Text File Creation is done!");
            return savePath;
        }
    }
}
