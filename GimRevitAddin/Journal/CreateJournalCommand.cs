namespace Gim.Revit.Addin.Journal
{
    using Autodesk.Revit.Attributes;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Forms;

    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    [Journaling(JournalingMode.UsingCommandData)]
    public class CreateJournalCommand : IExternalCommand
    {

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                var viewModel = new CreateJournalViewModel();
                var view = new CreateJournalView
                {
                    DataContext = viewModel
                };

                viewModel.SetDocumentationViewModel(view.DocumentationViewModel);

                var dialog = new Window
                {
                    Content = view,
                    SizeToContent = SizeToContent.WidthAndHeight
                };
                dialog.ShowDialog();

                if (viewModel.Result == DialogResult.OK)
                {
                    var application = commandData.Application.Application;
                    var manager = new FileCreationManager();
                    var settings = viewModel.Setting;
                    settings.SetRevitDirectory(application);
                    var journalFiles = new List<string>();
                    var count = 0;
                    foreach (var file in settings.DocumentationFiles)
                    {
                        settings.CurrentFile = file;
                        settings.FileCount = ++count;
                        manager.CreateJournal(settings);
                    }

                    manager.CreateBatch(settings);
                    manager.CreateAddinFile(settings);
                    return Result.Succeeded;
                }
                else
                {
                    return Result.Cancelled;
                }
            }
            catch (Exception ex)
            {
                message = $"Error: {ex.Message}\nStack: {ex.StackTrace}";
                return Result.Failed;
            }
        }
    }
}
