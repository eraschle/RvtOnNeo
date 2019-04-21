using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Gim.Revit.Addin.Docs.View;
using Gim.Revit.Addin.Journal;
using System;
using System.Windows;
using System.Windows.Forms;

namespace Gim.Revit.Addin.Docs
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    [Journaling(JournalingMode.UsingCommandData)]
    public class DocumentationCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var app = commandData.Application;
            if (app is null)
            {
                message = "No application object";
                return Result.Failed;
            }

            var uiDoc = app.ActiveUIDocument;
            if (uiDoc is null)
            {
                message = "No opent UIDocument";
                return Result.Failed;
            }

            var doc = uiDoc.Document;
            if (doc is null)
            {
                message = "No open Document";
                return Result.Failed;
            }

            if (doc.IsFamilyDocument == false)
            {
                message = "No family Document";
                return Result.Failed;
            }

            var manager = new CreateJournalManager(commandData);
            var viewModel = new DocumentationViewModel();
            if (manager.CanReadData == false)
            {
                var view = new DocumentationView
                {
                    DataContext = viewModel
                };

                var dialog = new Window
                {
                    Content = view,
                    SizeToContent = SizeToContent.WidthAndHeight
                };
                dialog.ShowDialog();
            }
            try
            {
                if (manager.CanReadData || viewModel.Result == DialogResult.OK)
                {
                    var setting = viewModel.Settings;
                    var tran = new Transaction(doc, "Family Documentation");
                    try
                    {
                        tran.Start();
                        manager.CreateDocumentation(doc, setting);
                        tran.Commit();
                        return Result.Succeeded;
                    }
                    catch (Exception ex)
                    {
                        tran.RollBack();
                        throw ex;
                    }
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
