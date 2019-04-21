using System;
using System.Collections.Generic;

namespace Gim.Revit.Addin.Journal
{
    public enum DocumentFormat
    {
        None = 0,
        Json = 1,
        Web = 2,
    }

    public class DocumentationSetting
    {
        internal const string DOCUMENTATION_KEY = "Documentation";
        internal const string FORMAT_JSON_KEY = "FormatJson";
        internal const string URL_KEY = "Url";
        internal const string API_KEY = "APiKey";

        public DocumentFormat DocumentFormat { get; set; }

        public bool FormatJson { get; set; }

        public bool WrapGimObjects { get; set; } = true;

        public string WebUrl { get; set; }

        public string ApiKey { get; set; }

        public IDictionary<string, string> GetJournalData()
        {
            var journalData = new Dictionary<string, string>
            {
                { DOCUMENTATION_KEY, DocumentFormat.ToString() },
                { FORMAT_JSON_KEY, FormatJson.ToString() },
                { URL_KEY, WebUrl },
                { API_KEY, ApiKey },
            };
            return journalData;
        }

        public void SetSettings(IDictionary<string, string> journalData)
        {
            SetDocumentFormat(journalData);
            SetJsonFormat(journalData);
            SetUrl(journalData);
            SetApiKey(journalData);
        }

        private void SetDocumentFormat(IDictionary<string, string> journalData)
        {
            var formatValue = GetSpecialData(journalData, DOCUMENTATION_KEY);
            DocumentFormat = DocumentFormat.None;
            foreach (var format in Enum.GetValues(typeof(DocumentFormat)))
            {
                if (formatValue.Equals(format.ToString()))
                {
                    DocumentFormat = (DocumentFormat)format;
                }
            }
        }

        private void SetJsonFormat(IDictionary<string, string> journalData)
        {
            var formatValue = GetSpecialData(journalData, FORMAT_JSON_KEY);
            FormatJson = bool.TryParse(formatValue, out var result) ? result : false;
        }

        private void SetUrl(IDictionary<string, string> journalData)
        {
            var url = GetSpecialData(journalData, URL_KEY);
            WebUrl = url;
        }

        private void SetApiKey(IDictionary<string, string> journalData)
        {
            var apiKey = GetSpecialData(journalData, API_KEY);
            ApiKey = apiKey;
        }

        /// <summary>
        /// Get the data which is related to a special key in journal
        /// </summary>
        /// <param name="dataMap">the map which store the journal data</param>
        /// <param name="key">a key which indicate which data to get</param>
        /// <returns>The gotten data in string format</returns>
        private static string GetSpecialData(IDictionary<string, string> dataMap, string key)
        {
            if (dataMap.ContainsKey(key) == false)
            {
                throw new Exception($"Journal data does NOT contain key '{key}.");
            }

            var dataValue = dataMap[key];
            return dataValue;
        }
    }
}
