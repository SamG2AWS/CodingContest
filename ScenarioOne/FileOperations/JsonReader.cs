using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace Scenario.FileOperations
{
    public class JsonReader : IJsonReader
    {
        private readonly ILogger<JsonReader> _logger;
        public JsonReader(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<JsonReader>();
        }

        public JObject ReadJsonFile(string filename)
        {
            JObject jObject = new JObject();
            try
            {
                jObject = JObject.Parse(File.ReadAllText(filename));
            }
            catch (Exception e)
            {
                _logger.LogDebug(e, $"Scenario.FileOperations.JsonReader: [ReadJsonFile] Error occured while parsing JSON file - {filename}");
            }

            return jObject;
        }
    }
}
