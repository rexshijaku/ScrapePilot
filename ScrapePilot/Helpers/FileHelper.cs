using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScrapePilot.Constants;
using ScrapePilot.Models;

namespace ScrapePilot.Helpers
{
    public class FileHelper
    {
        public static string? GetFile(string filePath)
        {
            try
            {
                string jsonContent = File.ReadAllText(filePath);
                return jsonContent;
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"File not found. {filePath}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return null;
        }

        public static void SetArgumentables(List<string> args, Dictionary<string, string> mapped)
        {
            for (int i = 0; i < args.Count(); i++)
            {
                if (args[i] == DependentFunctions.FileNameFromFullName)
                {
                    args[i] = Functions.FileName(mapped[DependentFunctions.FileNameFromFullName]);
                }
            }
        }

        public static string GetFragmented(List<string>? fragments)
        {
            if (fragments != null)
            {
                List<string> combine = new List<string>();

                foreach (var fragment in fragments)
                {
                    if (fragment == IndependentFunctions.DateTimeYYYYmmDD)
                    {
                        combine.Add(Functions.DateTimeyMd());
                        continue;
                    }

                    if (fragment == IndependentFunctions.Space)
                    {
                        combine.Add(Functions.Blank());
                        continue;
                    }

                    if (Store.IsAvailable(fragment))
                    {
                        combine.Add(Store.GetValue(fragment));
                    }
                    else
                    {
                        combine.Add(fragment);
                    }
                }

                return string.Join("", combine);
            }
            return "";
        }

        public static bool ValidDir(string? path)
        {
            return path != null && Directory.Exists(path);
        }
    }


}
