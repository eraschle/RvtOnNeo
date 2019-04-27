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

            SourceDirectoryCommand = new RelayCommand(SelectSource);
            DestinationDirectoryCommand = new RelayCommand(SelectDestination);

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
                if (value.Equals(sourcePath)) { return; }

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
                if (value.Equals(destinationPath)) { return; }

                destinationPath = value;
                NotifyPropertyChanged();
            }
        }

        public RelayCommand SourceDirectoryCommand { get; }

        public RelayCommand DestinationDirectoryCommand { get; }

        private void SelectSource(object parameter)
        {
            SourcePath = SelectDirectory(sourcePath);
            RefreshFileInfo();
        }

        private void RefreshFileInfo()
        {
            var setting = Setting;
            RefreshAllowed(setting);
            RefreshForbidden(setting);
            FileOverview = string.Empty;
        }

        private void SelectDestination(object parameter)
        {
            DestinationPath = SelectDirectory(destinationPath, true);
        }

        private string SelectDirectory(string currentDirectory, bool newButton = false)
        {
            var directory = currentDirectory;
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.SelectedPath = currentDirectory;
                dialog.ShowNewFolderButton = newButton;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    directory = dialog.SelectedPath;
                }
            }
            return directory;
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

        public object CommandParameter { get; set; }

        public bool CanExecute(object parameter)
        {
            var canExecute = AllowedFiles.Count > 0;
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
