using Newtonsoft.Json.Linq;

namespace Scenario.FileOperations
{
    public interface IJsonReader
    {
        JObject ReadJsonFile(string filename);
    }
}
