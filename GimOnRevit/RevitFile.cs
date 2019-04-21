namespace Gim.Revit
{
    using System.Collections.Generic;
    using System.IO;

    public class RevitFile
    {
        internal const string FAMILY_EXTENSION = "rfa";
        internal const string PROJECT_EXTENSION = "rvt";
        internal const string TEMPLATE_EXTENSION = "rte";

        public RevitFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                Name = Path.GetFileNameWithoutExtension(filePath);
                Extension = Path.GetExtension(filePath);
                FilePath = filePath;
            }
            else
            {
                Name = filePath;
                FilePath = filePath;
            }
        }

        public string Name { get; private set; }

        public string Extension { get; private set; }

        public string FilePath { get; private set; }

        public bool IsFamily { get { return IsExtension(FAMILY_EXTENSION); } }

        public bool IsProject { get { return IsExtension(PROJECT_EXTENSION); } }

        public bool IsTemplate { get { return IsExtension(TEMPLATE_EXTENSION); } }

        public override bool Equals(object obj)
        {
            return obj is RevitFile file &&
                   FilePath == file.FilePath;
        }

        public override int GetHashCode()
        {
            return 1230029444 + EqualityComparer<string>.Default.GetHashCode(FilePath);
        }

        private bool IsExtension(string extension)
        {
            return Extension.ToLower().EndsWith(extension);
        }


    }
}
