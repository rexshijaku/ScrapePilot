using System.Text.Json;

namespace ScrapePilot.Client.Helpers
{
    // This class helps with formatting JSON
    public static class JsonFormatter
    {
        public static string Format(string json)
        {
            try
            {
                using (JsonDocument doc = JsonDocument.Parse(json))
                {
                    return doc.RootElement.ToString();
                }
            }
            catch
            {
                return json;
            }
        }
    }
}
