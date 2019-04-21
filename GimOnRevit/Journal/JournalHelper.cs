using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Gim.Revit.Journal
{
    /// <summary>
    /// Automates Journal creation.
    /// </summary>
    public class JournalHelper
    {
        /// <summary>
        /// Builds the first part of the journal string
        /// </summary>
        /// <param name="debugMode">Should the journal file be run in debug mode?</param>
        /// <returns>The first part of the journal string.</returns>
        public static string BuildJournalStart(bool debugMode = false)
        {
            var journalStart = "'" +
                "Dim Jrn \n" +
                "Set Jrn = CrsJournalScript \n";

            if (debugMode)
            {
                journalStart += "Jrn.Directive \"DebugMode\", \"PerformAutomaticActionInErrorDialog\", 1 \n";
                journalStart += "Jrn.Directive \"DebugMode\", \"PermissiveJournal\", 1 \n";
            }
            return journalStart;
        }

        public static string BuildDocumentOpen(string revitFilePath, bool startInDefault3DView = false)
        {
            if (!File.Exists(revitFilePath))
            {
                throw new FileNotFoundException();
            }
            var projectOpen = $"Jrn.Command \"StartupPage\" , \"Open this project , ID_FILE_MRU_FIRST\" {Environment.NewLine}";
            projectOpen += $"Jrn.Data \"MRUFileName\" , \"{revitFilePath}\" {Environment.NewLine}";
            if (startInDefault3DView)
            {
                projectOpen += $"Jrn.Command \"Ribbon\" , \"Create a default 3D orthographic view. , ID_VIEW_DEFAULT_3DVIEW\"{Environment.NewLine}";
            }
            return projectOpen;
        }

        /// <summary>
        /// Builds the part of the journal string responsible for launching the Addin
        /// </summary>
        /// <param name="addinId">The Addin Id of the costum Addin.</param>
        /// <param name="commandNamespace">The command to start</param>
        /// <returns>The part of the journal string responsible for launching the Addin.</returns>
        public static string BuildAddinLaunch(string addinId, string commandNamespace)
        {
            return $"Jrn.RibbonEvent \"Execute external command:{addinId}:{commandNamespace}\"{Environment.NewLine}";
        }

        /// <summary>
        /// Builds the part of the journal string responsible to set the journal data of the command
        /// </summary>
        /// <param name="commandData">Dictonary with all command jounrnal data.</param>
        /// <returns>The part of the journal string responsible to set journal data of the command.</returns>
        public static string BuildAddinAddinCommandData(IDictionary<string, string> commandData)
        {
            var journalData = $"Jrn.Data \"APIStringStringMapJournalData\", {commandData.Keys.Count}";
            foreach (var key in commandData.Keys)
            {
                journalData += $", \"{key}\", \"{commandData[key]}\"";
            }
            return $"{journalData}{Environment.NewLine}";
        }


        public static int KillCurrentProcess()
        {
            var currentProcess = Process.GetCurrentProcess();
            currentProcess.Kill();
            return currentProcess.ExitCode;
        }
    }
}
