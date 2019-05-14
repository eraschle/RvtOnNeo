using Gim.Domain.Helpers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Gim.Domain.Writer
{
    public class GimJsonWriter
    {
        private readonly Formatting jsonFormat = Formatting.Indented;
        private const string nullValue = "\"NULL\"";

        public string GetFilePath(string dirctoryPath, string filePath)
        {
            filePath = FileHelper.RepairInvalidFileName(filePath);
            var directory = FileHelper.RepairInvalidPathName(dirctoryPath);
            return Path.Combine(directory, filePath);
        }


        public void WrtieFiles<T>(IList<string> filePaths, IList<T> jsonObjects) where T : class
        {
            for (var idx = 0; idx < jsonObjects.Count; idx++)
            {
                WrtieFile(filePaths[idx], jsonObjects[idx]);
            }
        }

        public void WrtieFile(string filePath, object jsonObject)
        {
            var jsonString = CreateJson(jsonObject);
            jsonString = string.IsNullOrEmpty(jsonString) ? jsonString
                : jsonString.Replace("null", nullValue);

            FileHelper.WriteFile(filePath, jsonString);
        }

        private string CreateJson(object serialzeObject)
        {
            var setting = new JsonSerializerSettings
            {
                Formatting = jsonFormat,
                NullValueHandling = NullValueHandling.Ignore,
                StringEscapeHandling = StringEscapeHandling.Default,
                Culture = CultureInfo.CurrentCulture
            };
            return JsonConvert.SerializeObject(serialzeObject, setting);
        }
    }
}
