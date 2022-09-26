using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Crosscutting.Common.Tools.Web
{
    public static class JsonHelper
    {
        public static string GetValueByKeyName(string content, string keyName)
        {
            var data = (JObject)JsonConvert.DeserializeObject(content);
            return data[keyName].Value<string>();
        }
    }
}
