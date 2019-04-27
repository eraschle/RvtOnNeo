using Gim.Domain.Helpers.Event;
using Gim.Revit.Addin.Helper;
using Gim.Revit.Addin.Journal;
using static Gim.Revit.Helper.FileHelper;

namespace Gim.Revit.Addin.Docs.View
{
    public class DocumentationViewModel 
        : ANotifyPropertyChangedModel, IDialogContentViewModel
    {
        public DocumentationViewModel()
        {
            isJson = true;
            isWeb = false;
            formatJson = true;
            windwosLineEnding = true;
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

        private bool macOsLineEnding;
        public bool MacOsLineEnding
        {
            get { return macOsLineEnding; }
            set
            {
                if (value == macOsLineEnding)
                {
                    return;
                }

                macOsLineEnding = value;
                NotifyPropertyChanged();
            }
        }

        private NewLine NewLineSymbol
        {
            get
            {
                return WindowLineEnding ? NewLine.Windows : 
                    LinuxLineEnding ? NewLine.Linux : 
                    MacOsLineEnding ? NewLine.MacOs : 
                    NewLine.None;
            }
        }

        public bool CanExecute
        {
            get
            {
                var canExecute = windwosLineEnding
                                || linuxLineEnding
                                || macOsLineEnding;
                if (IsWeb)
                {
                    canExecute &= string.IsNullOrEmpty(WebUrl) == false;
                    canExecute &= string.IsNullOrEmpty(ApiKey) == false;
                }
                return canExecute;
            }
        }

        public DocumentationSetting Settings
        {
            get
            {
                var setting = new DocumentationSetting
                {
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

        public object CommandParameter { get; set; }

        public bool CanExecute(object parameter)
        {
            var canExecute = true;
            if (IsWeb)
            {
                canExecute &= string.IsNullOrEmpty(WebUrl) == false;
                canExecute &= string.IsNullOrEmpty(ApiKey) == false;
            }
            return canExecute;
        }

        public void ExecuteOkay(object parameter) { }

        public void ExecuteCancel(object parameter) { }
    }
}
