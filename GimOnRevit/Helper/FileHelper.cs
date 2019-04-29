using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Gim.Revit.Helper
{
    public class FileHelper
    {

        public enum NewLine { None, Windows, Linux, MacOs }

        public static IDictionary<NewLine, string> NewLineSymbols { get; private set; }

        public static string ConvertNewLine(string content, NewLine fromSymbol, NewLine toSymbol)
        {
            if (string.IsNullOrEmpty(content)) { return content; }

            return content.Replace(NewLineSymbols[fromSymbol],
                                   NewLineSymbols[toSymbol]);
        }

        public static bool ContainSymbol(string content, NewLine symbol)
        {
            return string.IsNullOrEmpty(content) == false
                && content.Contains(NewLineSymbols[symbol]);
        }

        private static readonly HashSet<char> invalidPathChars = new HashSet<char> { '/' };

        static FileHelper()
        {
            NewLineSymbols = new Dictionary<NewLine, string>
            {
                { NewLine.None, string.Empty },
                { NewLine.Windows, "\r\n" },
                { NewLine.Linux, "\n" },
                { NewLine.MacOs, "\n" }
            };
        }

        public static bool IsValid(string filePath, char[] invalidChars, out IDictionary<int, char> invalidIndexCharDict)
        {
            var filePathChars = filePath.ToCharArray();
            var allInvalidChars = new List<char>(invalidChars);
            allInvalidChars.AddRange(invalidPathChars);
            invalidChars = allInvalidChars.ToArray();
            invalidIndexCharDict = new Dictionary<int, char>();
            for (var idx = 0; idx < filePathChars.Length; idx++)
            {
                var fileChar = filePathChars[idx];
                if (Array.Exists(invalidChars, c => c == fileChar))
                {
                    invalidIndexCharDict.Add(idx, fileChar);
                }
            }
            return invalidIndexCharDict.Count == 0;
        }

        public static bool IsValidFileName(string filePath, out IDictionary<int, char> invalidIndexCharDict)
        {
            invalidIndexCharDict = new Dictionary<int, char>();
            try
            {
                var invalidChars = Path.GetInvalidFileNameChars();
                //var fileName = Path.GetFileName(filePath);
                return IsValid(filePath, invalidChars, out invalidIndexCharDict);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool IsValidPathName(string filePath, out IDictionary<int, char> invalidIndexCharDict)
        {
            invalidIndexCharDict = new Dictionary<int, char>();
            try
            {
                var invalidChars = Path.GetInvalidPathChars();
                //var directoryPath = Path.GetDirectoryName(filePath);
                return IsValid(filePath, invalidChars, out invalidIndexCharDict);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string RepairInvalidFileName(string filePath, char replaceChar = '_')
        {
            if (IsValidFileName(filePath, out var invalidChars)) { return filePath; }

            return RepairInvalidChar(filePath, invalidChars, replaceChar);
        }

        public static string RepairInvalidPathName(string filePath, char replaceChar = '_')
        {
            if (IsValidPathName(filePath, out var invalidChars)) { return filePath; }

            return RepairInvalidChar(filePath, invalidChars, replaceChar);
        }

        private static string RepairInvalidChar(string filePath, IDictionary<int, char> invalidIndexCharMap, char replaceChar)
        {
            foreach (var idx in invalidIndexCharMap.Keys)
            {
                var invalidChar = invalidIndexCharMap[idx];
                filePath = filePath.Replace(invalidChar, replaceChar);
            }
            return filePath;
        }

        public static string ChangeExtension(string filePath, string extension)
        {
            var fileName = Path.GetFileName(filePath);
            var sourceName = Path.GetFileNameWithoutExtension(filePath);
            return filePath.Replace(fileName, $"{sourceName}.{extension}");
        }

        /// <summary>
        /// Deletes the journal file if it already exists.
        /// </summary>
        /// <param name="journalFilePath">The path of the generated journal file.</param>
        private static void DeleteFile(string journalFilePath)
        {
            if (File.Exists(journalFilePath))
            {
                File.Delete(journalFilePath);
            }
        }

        /// <summary>
        /// Writes the journal file.
        /// </summary>
        /// <param name="filePath">The path of the generated journal file.</param>
        /// <param name="fileContent">The string for the journal file.</param>
        public static void WriteFile(string filePath, string fileContent)
        {
            DeleteFile(filePath);
            var encoding = new UTF8Encoding(false);
            using (var tw = new StreamWriter(filePath, true, encoding))
            {
                try
                {
                    tw.Write(fileContent);
                }
                finally
                {
                    tw.Flush();
                }
            }
        }
    }
}
