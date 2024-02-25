using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScrapePilot.Constants;

namespace ScrapePilot
{
    public class Store
    {
        public static string KeyPrefix { get; set; } = "#";
        private static Dictionary<string, string> actionResults = new Dictionary<string, string>();

        public static string GetValue(string key)
        {
            return actionResults[key];
        }

        public static void Reset()
        {
            Console.WriteLine("Reseting the Store...");
            actionResults.Clear();
        }

        public static void SetValue(string? key, string? value, string errMsg = "")
        {

            if (String.IsNullOrEmpty(key))
            {
                App._consolePrinter.Print($"Invalid Argument Declaration in [{errMsg}] -> Key Was Not set.");
                return;
            }

            /*
             * Storing only those which begin with KeyPrefix
             */
            if (!MatchesPrefix(key))
            {
                App._consolePrinter.Print($"Invalid Key Name, the Prefix {KeyPrefix} was not specified, therefore not stored.");
                return;
            }

            if (value is null)
            {
                App._consolePrinter.Print($"Invalid Argument Declaration in Key [{key}] -> Value Was Not set.");
                return;
            }

            App._consolePrinter.Print($"Storing the result of Key [{key}]");

            actionResults[key] = value;
        }

        public static bool IsAvailable(string key)
        {
            return actionResults.ContainsKey(key);
        }

        public static bool MatchesPrefix(string key)
        {
            return key.StartsWith(KeyPrefix);
        }

    }
}
