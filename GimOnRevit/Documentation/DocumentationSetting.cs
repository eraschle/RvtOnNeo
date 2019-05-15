using System;
using System.Collections.Generic;
using System.IO;
using static Gim.Domain.Helpers.FileHelper;

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
        internal const string LIBRARY_ROOT = "LibraryRoot";
        internal const string OUTPUT_DIRECTORY = "OutputDirectory";
        internal const string DOCUMENTATION_KEY = "Documentation";
        internal const string FORMAT_JSON_KEY = "FormatJson";
        internal const string URL_KEY = "Url";
        internal const string API_KEY = "APiKey";
        internal const string NEW_LINE_SYMBOL = "LineEnding";
        internal const string EXPORT_FBX = "Fbx";

        public string LibraryRoot { get; set; }

        public string OutputDirectory { get; set; }

        public DocumentFormat DocumentFormat { get; set; }

        public NewLine NewLineSymbol { get; set; }

        public bool FormatJson { get; set; }

        public bool WrapGimObjects { get; set; } = true;

        public string WebUrl { get; set; }

        public string ApiKey { get; set; }

        public bool ExportFbx { get; set; }

        public IDictionary<string, string> GetJournalData()
        {
            var journalData = new Dictionary<string, string>
            {
                { LIBRARY_ROOT, LibraryRoot },
                { OUTPUT_DIRECTORY, OutputDirectory },
                { DOCUMENTATION_KEY, DocumentFormat.ToString() },
                { EXPORT_FBX, ExportFbx.ToString() },
                { FORMAT_JSON_KEY, FormatJson.ToString() },
                { URL_KEY, WebUrl },
                { API_KEY, ApiKey },
                { NEW_LINE_SYMBOL, NewLineSymbol.ToString() }
            };
            return journalData;
        }

        public void SetSettings(IDictionary<string, string> journalData)
        {
            SetLibraryRoot(journalData);
            SetOutputDir(journalData);
            SetDocumentFormat(journalData);
            SetExportFbx(journalData);
            SetJsonFormat(journalData);
            SetUrl(journalData);
            SetApiKey(journalData);
            SetNewLineSymbol(journalData);
        }

        public string CreateOutputFilePath(string filePath, string extension)
        {
            if (IsSubDirectory(filePath) == false)
            {
                var message = $"File path is NOT a subdirectory of {LibraryRoot}";
                throw new ArgumentException(message);
            }


            var outputSubDir = GetOutputDirectory(filePath);
            if (Directory.Exists(outputSubDir) == false)
            {
                Directory.CreateDirectory(outputSubDir);
            }

            var fileName = Path.GetFileName(filePath);
            var outputFilePath = Path.Combine(outputSubDir, fileName);
            return ChangeExtension(outputFilePath, extension);
        }

        public bool IsSubDirectory(string filePath)
        {
            try { return filePath.StartsWith(LibraryRoot); }
            catch { return false; }
        }

        private string GetOutputDirectory(string filePath)
        {
            var fileDirectory = Path.GetDirectoryName(filePath);
            var libraryRoot = $"{LibraryRoot}{Path.DirectorySeparatorChar}";
            var librarySubDir = fileDirectory.Replace(libraryRoot, "");

            return string.IsNullOrEmpty(librarySubDir)
                ? OutputDirectory
                : Path.Combine(OutputDirectory, librarySubDir);
        }

        private void SetLibraryRoot(IDictionary<string, string> journalData)
        {
            var libraryRoot = GetSpecialData(journalData, LIBRARY_ROOT);
            LibraryRoot = libraryRoot;
        }

        private void SetOutputDir(IDictionary<string, string> journalData)
        {
            var outputDir = GetSpecialData(journalData, OUTPUT_DIRECTORY);
            OutputDirectory = outputDir;
        }

        private void SetExportFbx(IDictionary<string, string> journalData)
        {
            var formatValue = GetSpecialData(journalData, EXPORT_FBX);
            FormatJson = Convert.ToBoolean(formatValue);
        }

        private void SetDocumentFormat(IDictionary<string, string> journalData)
        {
            DocumentFormat = DocumentFormat.None;

            var formatValue = string.Empty;
            try { formatValue = GetSpecialData(journalData, DOCUMENTATION_KEY); }
            catch { }

            if (string.IsNullOrEmpty(formatValue)) { return; }

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
            FormatJson = Convert.ToBoolean(formatValue);
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

        private void SetNewLineSymbol(IDictionary<string, string> journalData)
        {
            NewLineSymbol = NewLine.None;

            var newLineSymbol = string.Empty;
            try { newLineSymbol = GetSpecialData(journalData, NEW_LINE_SYMBOL); }
            catch { }

            if (string.IsNullOrEmpty(newLineSymbol)) { return; }

            foreach (var newLine in Enum.GetValues(typeof(NewLine)))
            {
                if (newLineSymbol.Equals(newLine.ToString()))
                {
                    NewLineSymbol = (NewLine)newLine;
                }
            }
        }

        /// <summary>
        /// Get the data which is related to a special key in journal
        /// </summary>
        /// <param name="dataMap">the map which store the journal data</param>
        /// <param name="key">a key which indicate which data to get</param>
        /// <returns>The gotten data in string format</returns>
        private string GetSpecialData(IDictionary<string, string> dataMap, string key)
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
