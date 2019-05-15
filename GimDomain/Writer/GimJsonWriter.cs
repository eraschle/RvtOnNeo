using Gim.Domain.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Gim.Domain.Writer
{
    public class GimJsonWriter
    {
        private const string quote = "\"";
        public bool Formating
        {
            get { return jsonFormating == Formatting.Indented; }
            set
            {
                jsonFormating = value == true 
                    ? Formatting.Indented 
                    : Formatting.None;
            }
        }

        private Formatting jsonFormating { get; set; }

        public GimJsonWriter() : this(true, "NULL") { }

        public GimJsonWriter(bool formatting, string nullValue)
        {
            Formating = formatting;
            NullValue = nullValue;
        }

        private string nullValue;
        public string NullValue
        {
            get { return nullValue; }
            set
            {
                if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                {
                    var message = "Replacment Value for null MUST have a value";
                    throw new ArgumentException(message);
                }
                nullValue = SuroundWithQuotes(value);
            }
        }

        private string SuroundWithQuotes(string value)
        {
            var builder = new StringBuilder(value);
            if (value.StartsWith(quote) == false)
            {
                builder.Insert(0, quote);
            }

            if (value.EndsWith(quote) == false)
            {
                builder.Append(quote);
            }
            return builder.ToString();
        }

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
                Formatting = jsonFormating,
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
