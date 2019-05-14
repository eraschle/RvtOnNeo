using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Autodesk.Revit.DB;
using Autodesk.RevitAddIns;
using Gim.Domain.Helpers;
using Gim.Revit.Helper;
using Gim.Revit.Helper.Journal;

namespace Gim.Revit.Addin.Journal
{
    public class FileCreationManager
    {
        private const string addinFileName = "GimRevitAddin.addin";

        private readonly IList<string> journalFiles = new List<string>();

        public void CreateJournal(CreateJournalSetting createSetting)
        {
            try
            {
                var journalData = JournalHelper.BuildJournalStart(createSetting.DebugModus);
                journalData += JournalHelper.BuildDocumentOpen(createSetting.CurrentFile.FilePath);

                journalData += JournalHelper.BuildAddinLaunch(createSetting.AddinId, createSetting.CmdFullName);
                journalData += JournalHelper.BuildAddinAddinCommandData(createSetting.JournalData);

                var journalFile = FileHelper.ChangeExtension(createSetting.CurrentFile.FilePath, "txt");
                journalFile = Path.GetFileName(journalFile);
                journalFile = $"{createSetting.FileCount}_{journalFile}";
                journalFile = Path.Combine(createSetting.DestinationDir, journalFile);
                FileHelper.WriteFile(journalFile, journalData);
                journalFiles.Add(journalFile);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CreateBatch(CreateJournalSetting setting)
        {
            var batchContent = string.Empty;
            batchContent += $"@echo off{Environment.NewLine}";
            batchContent += $"echo  {setting.JobName}{Environment.NewLine}";
            batchContent += Environment.NewLine;
            batchContent += $"cd {setting.RevitDirectory}{Environment.NewLine}";
            batchContent += Environment.NewLine;
            var count = 0;
            var fileCount = journalFiles.Count;
            foreach (var journalFile in journalFiles)
            {
                var fileName = Path.GetFileNameWithoutExtension(journalFile);
                batchContent += $"echo  {++count}/{fileCount} {fileName}{Environment.NewLine}";
                batchContent += $"start /W /HIGH Revit.exe {journalFile} /language {setting.Language} /nosplash{Environment.NewLine}";
                batchContent += Environment.NewLine;
            }
            batchContent += $"exit{Environment.NewLine}";
            FileHelper.WriteFile(setting.JobBatchFilePath, batchContent);
        }

        public void CreateAddinFile(CreateJournalSetting setting)
        {
            var fullPath = Assembly.GetAssembly(GetType()).Location;
            if (Guid.TryParse(setting.AddinId, out var addinGuid) == false)
            {
                throw new ArgumentException("No valid AddinId - can NOT be parsed to GUID");
            }

            var addinId = new AddInId(addinGuid);
            var currentManifest = GetCurrentManifest(fullPath, addinId);
            if (HasCommandId(currentManifest, setting.AddinId, out var command))
            {
                var newCommand = CreateNew(fullPath, command);
                var newManifest = new RevitAddInManifest();
                newManifest.AddInCommands.Add(newCommand);
                var newManifestPath = Path.Combine(setting.DestinationDir, addinFileName);
                newManifest.SaveAs(newManifestPath);
            }
        }

        private RevitAddInManifest GetCurrentManifest(string fullPathDll, AddInId addInId)
        {
            var dirPath = Path.GetDirectoryName(fullPathDll);
            var addins = AddInManifestUtility.GetRevitAddInManifests(dirPath, new Dictionary<string, string>());
            if (addins == null || addins.Count == 0) { return null; }

            RevitAddInManifest searchedAddin = null;
            var it = addins.GetEnumerator();
            while (it.MoveNext() && searchedAddin == null)
            {
                var addin = it.Current;
                var cmdIt = addin.AddInCommands.GetEnumerator();
                while (cmdIt.MoveNext() && searchedAddin == null)
                {
                    var cmdId = cmdIt.Current.AddInId;
                    if (cmdId.Equals(addInId.GetGUID()))
                    {
                        searchedAddin = addin;
                    }
                }
            }
            return searchedAddin is null ? new RevitAddInManifest() : searchedAddin;
        }

        private RevitAddInCommand CreateNew(string fullDllPat, RevitAddInCommand current)
        {
            var newCommand = new RevitAddInCommand(
                fullDllPat, current.AddInId, current.FullClassName, current.VendorId);

            return newCommand;

        }

        private bool HasCommandId(RevitAddInManifest manifest, string addinId, out RevitAddInCommand command)
        {
            command = null;
            foreach (var cmd in manifest.AddInCommands)
            {
                if (cmd.AddInId.ToString().Equals(addinId))
                {
                    command = cmd;
                    break;
                }
            }
            return command != null;
        }
    }
}
