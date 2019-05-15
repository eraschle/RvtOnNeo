using Autodesk.Revit.DB;
using Gim.Domain.Helpers;
using Gim.Domain.Helpers.Event;
using Gim.Revit.Addin.Helper;
using Gim.Revit.Addin.Journal;
using System.IO;
using System.Windows.Forms;
using static Gim.Domain.Helpers.FileHelper;

namespace Gim.Revit.Addin.Docs.View
{
    public class DocumentationViewModel
        : ANotifyPropertyChangedModel, IDialogContentViewModel
    {
        private string currentDocument = string.Empty;

        public DocumentationViewModel() : this(null) { }

        public DocumentationViewModel(Document document)
        {
            if (document != null)
            {
                currentDocument = Path.GetDirectoryName(document.PathName);
            }

            libraryRoot = currentDocument;
            outputDirectory = string.Empty;

            LibraryRootCommand = new RelayCommand(SelectLibraryRoot);
            OutputDirectoryCommand = new RelayCommand(SelectOutputDirectory);

            isJson = true;
            isWeb = false;
            formatJson = true;
            windwosLineEnding = true;
        }

        private string libraryRoot;
        public string LibraryRoot
        {
            get { return libraryRoot; }
            set
            {
                if (value.Equals(libraryRoot)) { return; }

                if (File.Exists(value))
                {
                    value = Path.GetDirectoryName(value);
                }

                libraryRoot = value;
                NotifyPropertyChanged();
            }
        }

        private string outputDirectory;
        public string OutputDirectory
        {
            get { return outputDirectory; }
            set
            {
                if (value.Equals(outputDirectory)) { return; }

                outputDirectory = value;
                NotifyPropertyChanged();
            }
        }

        public RelayCommand LibraryRootCommand { get; }

        public RelayCommand OutputDirectoryCommand { get; }

        private void SelectLibraryRoot(object parameter)
        {
            LibraryRoot = SelectDirectory(libraryRoot);
        }

        private void SelectOutputDirectory(object parameter)
        {
            OutputDirectory = SelectDirectory(outputDirectory, true);
        }

        private string SelectDirectory(string currentDirectory, bool newButton = false)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.SelectedPath = currentDirectory;
                dialog.ShowNewFolderButton = newButton;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    currentDirectory = dialog.SelectedPath;
                }
            }
            return currentDirectory;
        }

        private bool isJson;
        public bool IsJson
        {
            get { return isJson; }
            set
            {
                if (value == isJson) { return; }

                isJson = value;
                NotifyPropertyChanged();
            }
        }

        private bool isWeb;
        public bool IsWeb
        {
            get { return isWeb; }
            set
            {
                if (value == isWeb) { return; }

                isWeb = value;
                NotifyPropertyChanged();
            }
        }

        private string webUrl;
        public string WebUrl
        {
            get { return string.IsNullOrEmpty(webUrl) ? "Enter Url" : webUrl; }
            set
            {
                if (string.IsNullOrEmpty(value)) { return; }
                if (value.Equals(webUrl)) { return; }

                webUrl = value;
                NotifyPropertyChanged();
            }
        }

        private string apiKey;
        public string ApiKey
        {
            get { return string.IsNullOrEmpty(apiKey) ? "Enter API Key" : apiKey; }
            set
            {
                if (string.IsNullOrEmpty(value)) { return; }
                if (value.Equals(apiKey)) { return; }

                apiKey = value;
                NotifyPropertyChanged();
            }
        }

        private DocumentFormat DocumentFormat
        {
            get
            {
                return IsJson ? DocumentFormat.Json
                    : IsWeb ? DocumentFormat.Web : DocumentFormat.None;
            }
        }

        private bool formatJson;
        public bool FormatJson
        {
            get { return formatJson; }
            set
            {
                if (value == formatJson) { return; }

                formatJson = value;
                NotifyPropertyChanged();
            }
        }

        private bool windwosLineEnding;
        public bool WindowLineEnding
        {
            get { return windwosLineEnding; }
            set
            {
                if (value == windwosLineEnding)
                {
                    return;
                }

                windwosLineEnding = value;
                NotifyPropertyChanged();
            }
        }


        private bool linuxLineEnding;
        public bool LinuxLineEnding
        {
            get { return linuxLineEnding; }
            set
            {
                if (value == linuxLineEnding)
                {
                    return;
                }

                linuxLineEnding = value;
                NotifyPropertyChanged();
            }
        }

        private NewLine NewLineSymbol
        {
            get
            {
                return WindowLineEnding ? NewLine.Windows :
                    LinuxLineEnding ? NewLine.Linux : NewLine.None;
            }
        }

        private bool exportFbx;
        public bool ExportFbx
        {
            get { return exportFbx; }
            set
            {
                if (value == exportFbx)
                {
                    return;
                }

                exportFbx = value;
                NotifyPropertyChanged();
            }
        }


        public DocumentationSetting Settings
        {
            get
            {
                var setting = new DocumentationSetting
                {
                    LibraryRoot = LibraryRoot,
                    OutputDirectory = OutputDirectory,
                    ExportFbx = ExportFbx,
                    DocumentFormat = DocumentFormat,
                    FormatJson = IsJson ? FormatJson : false,
                    WebUrl = IsWeb ? WebUrl : string.Empty,
                    ApiKey = IsWeb ? ApiKey : string.Empty,
                    NewLineSymbol = NewLineSymbol,
                    WrapGimObjects = true
                };
                return setting;
            }
        }

        public object CommandParameter
        {
            get { return currentDocument; }
            set { }
        }

        public void ExecuteOkay(object parameter) { }

        public void ExecuteCancel(object parameter) { }

        public bool CanExecute(object parameter)
        {
            var currentPath = parameter as string;
            var canExecute = (windwosLineEnding || linuxLineEnding);
                // && DirectoryHelper.IsSameOrSub(LibraryRoot, currentPath);
            if (canExecute == true)
            {
                if (IsWeb)
                {
                    canExecute &= string.IsNullOrEmpty(WebUrl) == false;
                    canExecute &= string.IsNullOrEmpty(ApiKey) == false;
                }
                else if (IsJson)
                {
                    canExecute &= Directory.Exists(OutputDirectory);
                }
            }
            return canExecute;
        }
    }
}
