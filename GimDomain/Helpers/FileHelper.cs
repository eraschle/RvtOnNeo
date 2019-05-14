using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Gim.Domain.Helpers
{
    public class FileHelper
    {

        public enum NewLine { None, Windows, Linux }

        public static IDictionary<NewLine, string> NewLineSymbols { get; private set; }

        private static readonly HashSet<char> invalidPathChars = new HashSet<char> { '/' };

        private static readonly Encoding defaultEncoding = new UTF8Encoding(false);

        static FileHelper()
        {
            NewLineSymbols = new Dictionary<NewLine, string>
            {
                { NewLine.None, string.Empty },
                { NewLine.Windows, "\r\n" },
                { NewLine.Linux, "\n" },
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

        public static bool DeleteFile(string journalFilePath)
        {
            if (File.Exists(journalFilePath) == false) { return false; }

            try
            {
                File.Delete(journalFilePath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void WriteFile(string filePath, string fileContent, Encoding encoding = null)
        {
            DeleteFile(filePath);
            var fromLineEnd = NewLineSymbols[NewLine.Windows];
            var toLineEnd = NewLineSymbols[NewLine.Linux];

            var splits = new List<string> { fromLineEnd }.ToArray();
            var allLines = fileContent.Split(splits, StringSplitOptions.None);
            try
            {
                if (encoding is null)
                {
                    encoding = defaultEncoding;
                }

                using (var writer = new StreamWriter(filePath, append: false, encoding))
                {
                    writer.NewLine = toLineEnd;
                    for (var idx = 0; idx < allLines.Length; idx++)
                    {
                        var line = allLines[idx];
                        if (idx < allLines.Length - 1)
                        {
                            writer.WriteLine(line);
                        }
                        else
                        {
                            writer.Write(line);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"File {filePath}", ex);
            }
        }
    }
}
