using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using Gim.Domain.Helpers;
using Gim.Domain.Helpers.Event;
using Gim.Revit.Addin.Docs.View;
using Gim.Revit.Addin.Helper;

namespace Gim.Revit.Addin.Journal.View
{
    internal class CreateJournalViewModel
        : ANotifyPropertyChangedModel, IDialogContentViewModel
    {
        public CreateJournalViewModel()
        {
            sourcePath = Directories.UserHome;
            destinationPath = Directories.UserHome;

            allowedFiles = new ObservableCollection<RevitFile>();
            forbiddenFiles = new ObservableCollection<RevitFile>();
            recursive = true;
            debugModus = false;
        }

        private DocumentationViewModel docuViewModel;
        internal void SetDocumentationViewModel(DocumentationViewModel viewModel)
        {
            docuViewModel = viewModel;
        }

        private string sourcePath;
        public string SourcePath
        {
            get { return sourcePath; }
            set
            {
                if (sourcePath.Equals(value)) { return; }

                sourcePath = value;
                NotifyPropertyChanged();
            }
        }

        private string destinationPath;
        public string DestinationPath
        {
            get { return destinationPath; }
            set
            {
                if (destinationPath.Equals(value)) { return; }

                destinationPath = value;
                NotifyPropertyChanged();
            }
        }

        public RelayCommand SourceDirectoryCommand
        {
            get { return new RelayCommand(SelectSource); }
        }

        private void SelectSource(object parameter)
        {
            SourcePath = SelectDirectory(sourcePath, false);
            RefreshFileInfo();
        }

        public RelayCommand DestinationDirectoryCommand
        {
            get { return new RelayCommand(SelectDestination); }
        }

        private void SelectDestination(object parameter)
        {
            DestinationPath = SelectDirectory(destinationPath, true);
        }
        private void RefreshFileInfo()
        {
            var setting = Setting;
            RefreshAllowed(setting);
            RefreshForbidden(setting);
            FileOverview = string.Empty;
        }

        private string SelectDirectory(string currentDirectory, bool newFolderButton)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.SelectedPath = currentDirectory;
                dialog.ShowNewFolderButton = newFolderButton;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    currentDirectory = dialog.SelectedPath;
                }
            }
            return currentDirectory;
        }

        private bool recursive;
        public bool RecursiveSearch
        {
            get { return recursive; }
            set
            {
                if (value == recursive) { return; }

                recursive = value;
                NotifyPropertyChanged();
            }
        }

        private bool debugModus;
        public bool DebugModus
        {
            get { return debugModus; }
            set
            {
                if (value == debugModus) { return; }

                debugModus = value;
                NotifyPropertyChanged();
            }
        }

        public string FileOverview
        {
            get
            {
                var allowed = AllowedFiles.Count;
                var forbidden = ForbiddenFiles.Count;
                return $"File Overview Allowed: {allowed} Forbidden: {forbidden}";
            }
            set { NotifyPropertyChanged(); }
        }

        private ObservableCollection<RevitFile> allowedFiles;
        public ObservableCollection<RevitFile> AllowedFiles
        {
            get { return allowedFiles; }
            set
            {
                allowedFiles = value;
                NotifyPropertyChanged();
            }
        }
        private void RefreshAllowed(CreateJournalSetting setting)
        {
            var allwowed = CreateJournalManager.AllowedFiles(setting);
            AllowedFiles = new ObservableCollection<RevitFile>(allwowed);
        }

        private ObservableCollection<RevitFile> forbiddenFiles;
        public ObservableCollection<RevitFile> ForbiddenFiles
        {
            get { return forbiddenFiles; }
            set
            {
                forbiddenFiles = value;
                NotifyPropertyChanged();
            }
        }

        private void RefreshForbidden(CreateJournalSetting setting)
        {
            var forbidden = CreateJournalManager.ForbiddenFiles(setting);
            ForbiddenFiles = GetList(forbidden);
        }

        private ObservableCollection<RevitFile> GetList(ICollection<RevitFile> list)
        {
            return new ObservableCollection<RevitFile>(list);
        }

        public object CommandParameter
        {
            get { return SourcePath; }
            set { }
        }

        public bool CanExecute(object parameter)
        {
            var currentPath = parameter as string;
            var libraryRoot = docuViewModel.LibraryRoot;
            var canExecute = AllowedFiles.Count > 0;
                // && DirectoryHelper.IsSameOrSub(libraryRoot, currentPath);
            return canExecute && docuViewModel.CanExecute(parameter);
        }

        public void ExecuteOkay(object parameter) { }

        public void ExecuteCancel(object parameter) { }

        public CreateJournalSetting Setting
        {
            get
            {
                var jobName = "Create Family Documentation";
                var settings = CreateJournalSetting.CreateSetting(jobName, AllowedFiles);
                settings.SourceDir = SourcePath;
                settings.DestinationDir = DestinationPath;
                settings.RecursiveSearch = RecursiveSearch;
                settings.DebugModus = DebugModus;
                settings.Language = "DEU";
                settings.JournalData = docuViewModel.Settings.GetJournalData();
                return settings;
            }
        }
    }
}
