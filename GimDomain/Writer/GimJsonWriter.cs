using Gim.Domain.Helpers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Gim.Domain.Writer
{
    public class GimJsonWriter
    {
        private readonly Formatting jsonFormat = Formatting.Indented;
        private const string nullValue = "\"NULL\"";

        public void WrtieFiles<T>(
            IList<string> filePaths, IList<T> jsonObjects,
            ICollection<JsonConverter> converters = null) where T : class
        {
            for (var idx = 0; idx < jsonObjects.Count; idx++)
            {
                WrtieFile(filePaths[idx], jsonObjects[idx], converters);
            }
        }

        public void WrtieFile(string filePath, object jsonObject,
            ICollection<JsonConverter> converters = null)
        {
            var jsonString = CreateJson(jsonObject, converters);
            jsonString = string.IsNullOrEmpty(jsonString) ? jsonString
                : jsonString.Replace("null", nullValue);

            FileHelper.WriteFile(filePath, jsonString);
        }

        public string CreateJson(object serialzeObject, ICollection<JsonConverter> converters = null)
        {
            var setting = new JsonSerializerSettings
            {
                Formatting = jsonFormat,
                NullValueHandling = NullValueHandling.Ignore,
                StringEscapeHandling = StringEscapeHandling.Default,
                Culture = CultureInfo.CurrentCulture,
            };

            if (converters != null && converters.Count > 0)
            {
                setting.Converters = converters.ToArray();
            }
            return JsonConvert.SerializeObject(serialzeObject, setting);
        }
    }
}
