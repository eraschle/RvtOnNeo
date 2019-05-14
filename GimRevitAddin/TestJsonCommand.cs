using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;

namespace Gim.Revit.Addin
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    [Journaling(JournalingMode.UsingCommandData)]
    public class TestJsonCommand : IExternalCommand
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

            var document = uiDoc.Document;
            if (document is null)
            {
                message = "No open Document";
                return Result.Failed;
            }

            try
            {
                var controller = new TestJsonController();
                var tran = new Transaction(document, "Family Documentation");
                try
                {
                    tran.Start();
                    controller.CreateCategories(document);
                    controller.CreateBips(document);
                    controller.CreateUnits(document);
                    tran.Commit();
                    return Result.Succeeded;
                }
                catch (Exception ex)
                {
                    tran.RollBack();
                    throw ex;
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
