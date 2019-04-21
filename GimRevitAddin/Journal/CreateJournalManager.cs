using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Gim.Revit.Documentation;
using Gim.Revit.Helper.Journal;

namespace Gim.Revit.Addin.Journal
{

    public class CreateJournalManager
    {
        private readonly ExternalCommandData commandData;
        public bool CanReadData { get; private set; }

        // Methods
        /// <summary>
        /// The default constructor
        /// </summary>
        /// <param name="externalCommandData">the external command data which revit give RevitAPI</param>
        public CreateJournalManager(ExternalCommandData externalCommandData)
        {
            commandData = externalCommandData;
            CanReadData = (0 < externalCommandData.JournalData.Count) ? true : false;
        }

        /// <summary>
        /// This is the main deal method in this sample.
        /// It invoke methods to read and write journal data and create a wall using these data
        /// </summary>
        public void CreateDocumentation(Document document, DocumentationSetting setting)
        {
            var manager = new DocumentationManager();
            if (CanReadData)
            {
                setting.SetSettings(commandData.JournalData);
                setting = ReadJournalData(setting);
                if (setting.DocumentFormat == DocumentFormat.Json)
                {
                    manager.CreateJsonFamilyDoc(document, setting);
                }
                else if (setting.DocumentFormat == DocumentFormat.Web)
                {
                    manager.CreateWebFamilyDoc(document, setting);
                }

                if (setting.DocumentFormat != DocumentFormat.None)
                {
                    JournalHelper.KillCurrentProcess();
                }
            }
            else
            {
                if (setting.DocumentFormat == DocumentFormat.Json)
                {
                    manager.CreateJsonFamilyDoc(document, setting);
                }
                WriteJournalData(setting);
            }
        }

        /// <summary>
        /// Read the journal data from the journal.
        /// All journal data is stored in commandData.Data.
        /// </summary>
        private DocumentationSetting ReadJournalData(DocumentationSetting setting)
        {
            var journalData = commandData.JournalData;
            setting.SetSettings(journalData);
            return setting;
        }

        /// <summary>
        /// Write the support data into the journal
        /// </summary>
        private void WriteJournalData(DocumentationSetting setting)
        {
            var dataMap = commandData.JournalData;
            dataMap.Clear();
            var journalData = setting.GetJournalData();
            foreach (var key in journalData.Keys)
            {
                dataMap.Add(key, journalData[key]);
            }
        }

        public static ICollection<RevitFile> AllowedFiles(CreateJournalSetting setting)
        {
            var allFiles = GetFoundFiles(setting);
            return allFiles.Where(filePath => IsAllowedForJournal(filePath))
                .Select(filePath => new RevitFile(filePath)).ToList();
        }

        public static ICollection<RevitFile> ForbiddenFiles(CreateJournalSetting setting)
        {
            var allFiles = GetFoundFiles(setting);
            return allFiles.Where(filePath => IsAllowedForJournal(filePath) == false)
                .Select(filePath => new RevitFile(filePath)).ToList();
        }

        public static bool IsAllowedForJournal(string filePath)
        {
            // This regular expression finds all but the following characters:
            // 0-1 a-z A-Z (no accented characters like Umlaute)
            // also _ - + { } ( ) [ ] . : \ , °
            var regexpat = "[^0-9\\u0041-\\u005A\\u0061-\\u007A-_+,°:{}()\\[\\]\\.\\\\]";
            var allowedRegex = new Regex(regexpat);
            var matches = allowedRegex.Matches(filePath);
            return matches.Count == 0;
        }

        public static IList<string> GetFoundFiles(CreateJournalSetting setting, string extension = "rfa")
        {
            var searchOption = SearchOption.TopDirectoryOnly;
            if (setting.RecursiveSearch)
            {
                searchOption = SearchOption.AllDirectories;
            }
            var filePaths = Directory.GetFiles(setting.SourceDir, $"*.{extension}", searchOption);
            return filePaths;
        }
    }
}
