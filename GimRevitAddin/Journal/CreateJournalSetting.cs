namespace Gim.Revit.Addin.Journal
{
    using System.Collections.Generic;
    using System.IO;
    using Autodesk.Revit.ApplicationServices;

    public class CreateJournalSetting
    {
        public static CreateJournalSetting CreateSetting(string jobName, IEnumerable<RevitFile> files)
        {
            return new CreateJournalSetting
            {
                AddinId = "12dae19d-edec-423c-bf70-293db65e04fd",
                CmdFullName = typeof(DocumentationCommand).FullName,
                JobName = jobName,
                DocumentationFiles = files
            };
        }

        private CreateJournalSetting() { }

        public string AddinId { get; private set; }

        public string CmdFullName { get; private set; }

        public string JobName { get; private set; }

        public string JobBatchFileName
        {
            get
            {
                return JobName.Replace(" ", string.Empty);
            }
        }

        public string JobBatchFilePath
        {
            get
            {
                var batchFileName = $"0_{JobBatchFileName}.bat";
                return Path.Combine(DestinationDir, batchFileName);
            }
        }

        public string RevitDirectory { get; private set; }

        internal void SetRevitDirectory(Application application)
        {
            var programm = EnvVar.ByName("ProgramW6432");
            var version = application.VersionNumber;
            var path = Path.Combine(programm, "Autodesk", $"Revit {version}");
            RevitDirectory = path;
        }

        public string SourceDir { get; set; }

        public string DestinationDir { get; set; }

        public bool RecursiveSearch { get; set; }

        public object Language { get; set; }

        public bool DebugModus { get; set; }

        public RevitFile CurrentFile { get; set; }

        public int FileCount { get; set; }

        public IEnumerable<RevitFile> DocumentationFiles { get; private set; }

        public IDictionary<string, string> JournalData { get; internal set; }
    }
}
