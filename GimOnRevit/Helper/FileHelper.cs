using System.IO;

namespace Gim.Revit.Helper
{
    public class FileHelper
    {
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
            using (var tw = new StreamWriter(filePath, true))
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
